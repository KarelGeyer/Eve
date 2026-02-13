using System.Net.Http.Json;
using System.Security.Cryptography;
using Common.Shared.Exceptions;
using Common.Shared.Helpers;
using Common.Shared.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Application.Interfaces;
using Users.Application.Models;
using Users.Domain.Interfaces.Reposiroties;

namespace Users.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly HttpClient _httpClient;
        private readonly ExternalSettings _externalSettings;

        public UserRegistrationService(
            IUserRepository userRepository,
            IEmailService emailService,
            HttpClient httpClient,
            IOptions<ExternalSettings> externalSettings
        )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _httpClient = httpClient;
            _externalSettings = externalSettings.Value;
        }

        /// <inheritdoc />
        public async Task<int> ActivateUserAsync(string token, CancellationToken ct)
        {
            int affectedRows = await _userRepository.ActivateUserAsync(token, ct);

            if (affectedRows == 0)
            {
                throw new SecurityException("Activation failed. Link is invalid, expired or was already used.");
            }

            return affectedRows;
        }

        /// <inheritdoc />
        public async Task<bool> IsEmailAdressBanned(string email, CancellationToken ct)
        {
            var result = await _httpClient.GetFromJsonAsync<AbstractEmailResponseDto>(
                $"{_externalSettings.AbstractApiUrl}?api_key={_externalSettings.AbstractApiKey}&email={email}",
                ct
            );

            if (
                result == null
                || result.Deliverability.StatusDetail == Common.Shared.Constants.Security.IsValidEmail
                || !result.Quality.IsLiveSite
                || result.Quality.IsDisposable
            )
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> IsEmailUsed(string email, CancellationToken ct) =>
            await _userRepository.DoesUserWithEmailExist(email, ct);

        /// <inheritdoc />
        public async Task<bool> IsUsernameUsed(string username, CancellationToken ct) =>
            await _userRepository.DoesUserWithUsernameExist(username, ct);

        /// <inheritdoc />
        public async Task<bool> RegisterUserAsync(UserRegistrationRequestDto request, CancellationToken ct)
        {
            if (request.AgreedToTos == false || request.AgreedToGdpr == false)
            {
                throw new SecurityException("User must agree to Terms of Service and GDPR policies to register.");
            }

            if (await IsEmailAdressBanned(request.Email, ct))
            {
                throw new SecurityException("Email failed background check, its either invalid or banned");
            }

            if (await DoesUserExist(request.Email, request.Username, ct))
            {
                var user = await _userRepository.GetFullUserAsync(request.Email, ct);
                bool isUserActive = user != null && user.Settings != null && user.Settings.IsActive;

                if (isUserActive)
                {
                    throw new EntityAlreadyExistsException(nameof(User), request.Email, "email");
                }
                else
                {
                    _userRepository.Delete(user!);
                    var userDeleted = await _userRepository.SaveChangesAsync(ct);
                    if (userDeleted == false)
                    {
                        throw new ActionFailedException(
                            nameof(RegisterUserAsync),
                            "Failed to delete existing user, please try again later."
                        );
                    }
                }
            }

            var activationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var userCreated = await CreateUser(request, activationToken, ct);

            if (userCreated == false)
            {
                throw new ActionFailedException(nameof(RegisterUserAsync), "Failed to create user, please try again later.");
            }

            string activationLink =
                $"{Common.Shared.Constants.Global.DevUrl}{Common.Shared.Constants.Global.ActivationEndpoint}?token={activationToken}";
            string template = await _emailService.GetEmailTemplate("ActivationEmail.html", ct);
            string htmlBody = template.Replace("{{ActivationLink}}", activationLink);

            await _emailService.SendEmailAsync(to: request.Email, subject: "Activate your account", body: htmlBody, ct: ct);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> ResendActivationEmailAsync(string email, CancellationToken ct)
        {
            var user = await _userRepository.GetFullUserAsync(email, ct);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User), $"User with email {email} not found");
            }

            if (user.Settings == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User Settings with userId {user.Id} not found");
            }

            if (user.Settings.IsActive)
            {
                throw new SecurityException(nameof(User), "User is already active.");
            }

            var activationToken = user.Settings.ActivationToken;

            if (activationToken == null)
            {
                throw new SecurityException(nameof(UserSettings), $"Activation token for user with email {email} not found");
            }

            string activationLink =
                $"{Common.Shared.Constants.Global.DevUrl}{Common.Shared.Constants.Global.ActivationEndpoint}?token={activationToken}";
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ActivationEmail.html");
            string template = await File.ReadAllTextAsync(templatePath, ct);
            string htmlBody = template.Replace("{{ActivationLink}}", activationLink);

            await _emailService.SendEmailAsync(to: email, subject: "Activate your account", body: htmlBody, ct: ct);

            return true;
        }

        /// <summary>
        /// Finds if there is already a user with the same email or username as the one provided in the registration request.
        /// This is used to prevent multiple accounts with the same email or username.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="ct"></param>
        /// <returns>True if User with provided credentials exists</returns>
        private async Task<bool> DoesUserExist(string email, string username, CancellationToken ct)
        {
            bool doesUserWithEmailExist = await _userRepository.DoesUserWithEmailExist(email, ct);
            bool doesUserWithUsernameExist = await _userRepository.DoesUserWithUsernameExist(username, ct);

            return doesUserWithEmailExist || doesUserWithUsernameExist;
        }

        /// <summary>
        /// Used to create a new user in the database with the provided registration details and generated activation token.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="activationToken"></param>
        /// <param name="ct"></param>
        /// <returns>True if the user was created in the database</returns>
        private async Task<bool> CreateUser(UserRegistrationRequestDto request, string activationToken, CancellationToken ct)
        {
            User existingNewUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                PasswordHash = PasswordManager.Hash(request.Password),

                Settings = new UserSettings
                {
                    ActivationToken = activationToken,
                    ActivationTokenExpiration = DateTime.UtcNow.AddHours(24),
                    PrefferedLanguage = request.PrefferedLanguage,
                    Theme = 0,
                    Timezone = "UTC",
                    SecurityQuestion = request.SecurityQuestion,
                    SecurityAnswer = request.SecurityAnswer,
                },
                Identity = new UserIdentity { },
                GDPR = new GDPR { TosAcceptAt = DateTime.UtcNow, TosVersion = "Todo: add a way to retrieve current version" },
            };

            _userRepository.Add(existingNewUser);

            bool userUpdateSuccesfull = await _userRepository.SaveChangesAsync(ct);

            return userUpdateSuccesfull;
        }
    }
}
