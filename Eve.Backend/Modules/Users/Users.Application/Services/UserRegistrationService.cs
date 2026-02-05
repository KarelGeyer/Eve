using System;
using System.Collections.Generic;
using System.Text;
using Common.Shared.Helpers;
using Common.Shared.Interfaces;
using Domain.Entities;
using Users.Application.Dtos.Requests;
using Users.Application.Interfaces;
using Users.Domain.Exceptions;
using Users.Domain.Interfaces.Reposiroties;

namespace Users.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserRegistrationService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public Task<bool> ActivateUserAsync(string token, string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailUsed(string email, CancellationToken ct) =>
            await _userRepository.DoesUserWithEmailExist(email, ct);

        public async Task<bool> IsUsernameUsed(string username, CancellationToken ct) =>
            await _userRepository.DoesUserWithUsernameExist(username, ct);

        public async Task<bool> RegisterUserAsync(UserRegistrationRequestDto request, CancellationToken ct)
        {
            if (request.AgreedToTos == false || request.AgreedToGdpr == false)
            {
                throw new ArgumentException("User must agree to Terms of Service.");
            }

            if (await DoesUserExist(request.Email, request.Username, ct))
            {
                throw new EntityAlreadyExistsException(nameof(User), request.Email);
            }

            // todo: do blacklist check
            // todo: do Rate Limit check

            var hashedPassword = PasswordManager.Hash(request.Password);

            User newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,

                Settings = new UserSettings(),
                Identity = new UserIdentity { PasswordHash = hashedPassword },
                GDPR = new GDPR { TosAcceptAt = DateTime.Now, TosVersion = "Todo: add a way to retrieve current version" },
            };

            var response = _userRepository.Add(newUser);

            if (response == null)
            {
                return false;
            }

            bool isUserCreated = await _userRepository.SaveChangesAsync(ct);

            if (!isUserCreated)
            {
                return false;
            }

            // todo: create template that will send activation link with token
            await _emailService.SendEmailAsync(
                to: request.Email,
                subject: "Activate your account",
                body: "Please click the link to activate your account: [activation link here]",
                ct: ct
            );

            return true;
        }

        public Task<bool> ResendActivationEmailAsync(string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> DoesUserExist(string email, string username, CancellationToken ct)
        {
            bool doesUserWithEmailExist = await _userRepository.DoesUserWithEmailExist(email, ct);
            bool doesUserWithUsernameExist = await _userRepository.DoesUserWithUsernameExist(username, ct);

            return doesUserWithEmailExist || doesUserWithUsernameExist;
        }
    }
}
