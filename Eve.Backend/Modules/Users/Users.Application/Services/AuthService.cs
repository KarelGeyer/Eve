using Common.Shared.Exceptions;
using Common.Shared.Helpers;
using Common.Shared.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;
using Users.Application.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Interfaces.Reposiroties;
using Users.Domain.Interfaces.Repositories;

namespace Users.Application.Services
{
    public class AuthService : IAuthService
    {
        IUserRepository _userRepository;
        ISessionRepository _sessionRepostitory;
        IDeviceDetector _deviceDetector;
        IUserContextService _userContextService;
        IJwtProvider _jwtProvider;
        IEmailService _emailService;

        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _deviceLocks = new ConcurrentDictionary<Guid, SemaphoreSlim>();

        public AuthService(
            IUserRepository userRepository,
            ISessionRepository sessionRepostitory,
            IDeviceDetector deviceDetector,
            IUserContextService userContextService,
            IJwtProvider jwtProvider,
            IEmailService emailService
        )
        {
            _userRepository = userRepository;
            _sessionRepostitory = sessionRepostitory;
            _deviceDetector = deviceDetector;
            _userContextService = userContextService;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct)
        {
            // Lock for race condition when multiple login attempts are made with the same device UUID at the same time
            var semaphore = _deviceLocks.GetOrAdd(request.DeviceUUID, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync(ct);

            try
            {
                var user = await _userRepository.GetFullUserAsync(request.Email, ct);
                var createdAt = DateTime.UtcNow;

                await ValidateUserLoginAttempt(user, request.Email, request.Password, request.Platform.ToString(), createdAt, ct);

                var refreshToken = GenerateRefreshToken();
                string jwtToken;
                bool shouldSendMaxSessionsEmail = false;

                var session = await _sessionRepostitory.GetUserSessionAsync(request.DeviceUUID, ct);
                if (session != null)
                {
                    bool updated = await ManageUpdateSessionAsync(session, refreshToken, createdAt, ct);
                    jwtToken = _jwtProvider.GenerateAccessToken(user!, session.SessionId.ToString());
                }
                else
                {
                    List<UserSession>? sessions = await _sessionRepostitory.GetUserSessionsAsync(user!.Id, ct);
                    if (sessions != null && sessions.Count >= 5)
                        shouldSendMaxSessionsEmail = ManageDeleteSessionAsync(sessions);

                    var newSession = new UserSession
                    {
                        SessionId = Guid.NewGuid(),
                        UserId = user.Id,
                        CreatedAt = createdAt,
                        Device = request.Platform.ToString(),
                        DeviceId = request.DeviceUUID,
                        DeviceName = request.DeviceName ?? string.Empty,
                        LastIpAddress = _userContextService.GetIpAddress(),
                        RefreshTokenHash = HashRefreshToken(refreshToken),
                        RefreshTokenExpiry = createdAt.AddDays(7),
                        IsActive = true,
                    };

                    _sessionRepostitory.Add(newSession);
                    bool result = await _sessionRepostitory.SaveChangesAsync(ct);

                    if (result == true)
                    {
                        await SendNewLoginEmail(user.Email, newSession.Device, newSession.DeviceName, newSession.LastIpAddress, newSession.CreatedAt.ToString("g"), ct);
                        if (shouldSendMaxSessionsEmail)
                        {
                            await SendMaxSessionsReachedEmail(user.Email, ct);
                        }

                        jwtToken = _jwtProvider.GenerateAccessToken(user, newSession.SessionId.ToString());
                    }
                    else
                    {
                        throw new ActionFailedException(
                            nameof(_sessionRepostitory.SaveChangesAsync),
                            "New session could not be signed in"
                        );
                    }
                }

                return new LoginResponseDto
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken,
                    Username = user!.Username,
                };
            } 
            finally
            {
                // Note!: There is a memory leak waiting to blow up ofcourse but given this app is created for my family, maybe friends and potentially monetization purposes for 10k unique UUID's
                // (And I am very pesimistic about this number), fuck it. Even if server is never restarted and all those data rot in there, for 10k UUID's is aproximatelly 20MB.
                semaphore.Release();
            }
        }
         

        public async Task<bool> LogoutAsync(Guid deviceUuid, CancellationToken ct)
        {
            var session = await _sessionRepostitory.GetUserSessionAsync(deviceUuid, ct);

            if(session == null)
            {
                throw new EntityNotFoundException(nameof(UserSession), "Session nebyla nalezena");
            }

            _sessionRepostitory.Delete(session);

            return await _sessionRepostitory.SaveChangesAsync(ct);
        }

        public Task<bool> RecoverPassword(string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponseDto> RefreshSessionAsync(RefreshRequestDto request, CancellationToken ct)
        {
            var semaphore = _deviceLocks.GetOrAdd(request.DeviceUUID, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync(ct);

            try 
            {
                var session = await _sessionRepostitory.GetUserSessionAsync(request.DeviceUUID, ct);
                if(session == null || !session.IsActive)
                {
                    throw new UnauthorizedAccessException("Session not found for the provided device UUID or is not active");
                }

                if(session.RefreshTokenHash == null || session.RefreshTokenExpiry < DateTime.Now)
                {
                    throw new UnauthorizedAccessException("Session has ended, new login is required");
                }

                bool isRefreshTokenValid = CompareRefreshTokens(request.RefreshToken, session.RefreshTokenHash);

                if (!isRefreshTokenValid)
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }

                var user = await _userRepository.GetAsync(session.UserId, ct);
                if(user == null)
                {
                    throw new EntityNotFoundException(nameof(User), "User no longer exists");
                }

                var refreshToken = GenerateRefreshToken();

                session.RefreshTokenHash = HashRefreshToken(refreshToken);
                session.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                _sessionRepostitory.Update(session);

                bool response = await _sessionRepostitory.SaveChangesAsync(ct);

                if (!response)
                {
                    throw new ActionFailedException(nameof(_sessionRepostitory.SaveChangesAsync), "Refresh token could not be updated.");
                }

                var jwtToken = _jwtProvider.GenerateAccessToken(user, session.SessionId.ToString());

                return new LoginResponseDto
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken,
                    Username = user.Username
                };
            } 
            finally 
            { 
                semaphore.Release(); 
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private string HashRefreshToken(string refreshToken)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(refreshToken);
            var hash = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        private bool CompareRefreshTokens(string providedToken, string storedHashedToken)
        {
            var hashedProvidedToken = HashRefreshToken(providedToken);
            byte[] b1 = Encoding.UTF8.GetBytes(hashedProvidedToken);
            byte[] b2 = Encoding.UTF8.GetBytes(storedHashedToken);

            return CryptographicOperations.FixedTimeEquals(b1, b2);
        }

        private async Task<bool> ManageUpdateSessionAsync(UserSession session, string newRefreshToken, DateTime date, CancellationToken ct)
        {
            session.RefreshTokenHash = HashRefreshToken(newRefreshToken);
            session.RefreshTokenExpiry = date.AddDays(7);
            _sessionRepostitory.Update(session);
            bool isUpdated = await _sessionRepostitory.SaveChangesAsync(ct);

            if (!isUpdated)
            {
                throw new ActionFailedException(nameof(_sessionRepostitory.SaveChangesAsync), "Session could not be updated");
            }

            return isUpdated;
        }

        private bool ManageDeleteSessionAsync(List<UserSession> sessions)
        {
            var oldestSession = sessions.OrderBy(s => s.CreatedAt).FirstOrDefault();
            bool isDeleted = _sessionRepostitory.Delete(oldestSession!);

            if (!isDeleted)
            {
                throw new ActionFailedException(nameof(_sessionRepostitory.Delete), "Old session could not be deleted");
            }

            return isDeleted;
        }

        private async Task SendMaxSessionsReachedEmail(string email, CancellationToken ct)
        {
            string template = await _emailService.GetEmailTemplate("MaxSessionsReachedEmail.html", ct);
            await _emailService.SendEmailAsync(to: email, subject: "New login on your account", body: template, ct: ct);
        }

        private async Task SendNewLoginEmail(string email, string device, string deviceName, string ip, string createdAt, CancellationToken ct)
        {
            string template = await _emailService.GetEmailTemplate("NewLoginEmail.html", ct);
            string htmlBody = template
                .Replace("{{DeviceType}}", device)
                .Replace("{{DeviceName}}", deviceName ?? "Neznámé zařízení")
                .Replace("{{IpAddress}}", ip)
                .Replace("{{DateTime}}", createdAt);

            await _emailService.SendEmailAsync(
                to: email,
                subject: "New login on your account",
                body: htmlBody,
                ct: ct
            );
        }

        private async Task ValidateUserLoginAttempt(User? user, string email, string password, string platform, DateTime date, CancellationToken ct)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException($"Invalid email or password");
            }

            if (user.Identity == null)
            {
                throw new EntityNotFoundException(nameof(UserIdentity), $"User Identity with user id {user.Id} not found.");
            }

            if (user.Settings == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User Settings with user id {user.Id} not found.");
            }

            if (user.Settings.IsEmailVerified == false)
            {
                throw new ForbidenException(user.Email);
            }

            if (user.Settings.IsBlocked)
            {
                if (user.Settings.UnblockDateTime <= DateTime.UtcNow)
                {
                    user.Settings.IsBlocked = false;
                    user.Settings.LoginAttempts = 0;

                    _userRepository.UpdateUserSettings(user.Settings);
                    await _userRepository.SaveChangesAsync(ct);
                }
                else
                {
                    throw new AccountLockedException(user.Id);
                }
            }

            if (!PasswordManager.Verify(password, user.PasswordHash))
            {
                if (user.Settings.LoginAttempts >= 5)
                {
                    user.Settings.IsBlocked = true;
                    user.Settings.UnblockDateTime = date.AddHours(1);

                    _userRepository.UpdateUserSettings(user.Settings);
                    await _userRepository.SaveChangesAsync(ct);

                    throw new AccountLockedException(user.Id);
                }
                else
                {
                    user.Settings.LoginAttempts += 1;

                    _userRepository.UpdateUserSettings(user.Settings);
                    await _userRepository.SaveChangesAsync(ct);

                    throw new UnauthorizedAccessException($"Invalid email or password");
                }
            }

            var deviceType = _deviceDetector.GetDeviceType();
            if (platform != deviceType.Platform)
            {
                throw new SecurityException("Device type does not match the one detected from the request");
            }
        }
    }
}
