using Common.Shared.Exceptions;
using Common.Shared.Helpers;
using Domain.Entities;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;
using Users.Application.Extensions;
using Users.Application.Interfaces;
using Users.Domain.Exceptions;
using Users.Domain.Interfaces.Reposiroties;

namespace Users.Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<UserBasicResponseDto> GetUserAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetAsync(id, ct);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User), $"User with Id {id}  not found");
            }

            if (user.Settings == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User Settings with userId {id}  not found");
            }

            return user.ToBasicDto();
        }

        /// <inheritdoc />
        public async Task<UserFullResponseDto> GetUserFullAsync(int id, CancellationToken ct)
        {
            var user = await _userRepository.GetAsync(id, ct);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User with id {id}  not found");
            }

            if (user.Settings == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User Settings with userId {id}  not found");
            }

            if (user.Identity == null)
            {
                throw new EntityNotFoundException(nameof(UserIdentity), $"User Identity with userId {id}  not found");
            }

            var subId = string.Empty;

            if (user.Identity.GoogleSubId != null)
                subId = user.Identity.GoogleSubId;
            if (user.Identity.AppleSubId != null)
                subId = user.Identity.AppleSubId;

            return user.ToFullDto(subId);
        }

        /// <inheritdoc />
        public async Task<UserIdentityResponseDto> GetUserIdentity(int id, CancellationToken ct)
        {
            var identity = await _userRepository.GetUserIdentityAsync(id, ct);

            if (identity == null)
            {
                throw new EntityNotFoundException(nameof(UserIdentity), $"User Identity with userId {id}  not found");
            }

            var subId = string.Empty;

            if (identity.GoogleSubId != null)
                subId = identity.GoogleSubId;
            if (identity.AppleSubId != null)
                subId = identity.AppleSubId;

            return identity.ToIdentityDto(subId);
        }

        /// <inheritdoc />
        public async Task<UserSettingsResponseDto> GetUserSettings(int id, CancellationToken ct)
        {
            var settings = await _userRepository.GetUserSettingsAsync(id, ct);

            if (settings == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User Settings with userId {id}  not found");
            }

            return settings.ToSettingsDto();
        }

        // soft delete
        // todo - create scheduler to delete soft deleted users after 30 days
        /// <inheritdoc />
        public async Task<bool> DeleteUserAsync(int id, string password, CancellationToken ct)
        {
            var user = await _userRepository.GetFullUserAsync(id, ct);

            if (user == null)
                return false;

            if (user.Settings == null)
            {
                throw new EntityNotFoundException(nameof(UserSettings), $"User Settings with userId {id} not found");
            }

            if (user.Identity == null)
            {
                throw new EntityNotFoundException(nameof(UserIdentity), $"User Identity with userId {id} not found");
            }

            if (!PasswordManager.Verify(password, user.Identity.PasswordHash))
                return false;

            user.Settings.IsActive = false;

            bool updated = _userRepository.Update(user);
            if (!updated)
                return false;

            return await _userRepository.SaveChangesAsync(ct);
        }

        // todo: dodělej validaci vstupních dat
        /// <inheritdoc />
        public async Task<UserUpdateResponseDto?> UpdateUserAsync(int id, UpdateProfileRequestDto request, CancellationToken ct)
        {
            var user = await _userRepository.GetFullUserAsync(id, ct);

            if (user == null)
                throw new EntityNotFoundException(nameof(User), $"User with id {id} not found");

            if (user.Identity == null)
            {
                throw new EntityNotFoundException(nameof(UserIdentity), $"User Identity with userId {id} not found");
            }

            if (!PasswordManager.Verify(request.CurrentPassword, user.Identity.PasswordHash))
                return null;

            user.Email = request.NewEmail ?? user.Email;
            user.PhoneNumber = request.NewPhoneNumber ?? user.PhoneNumber;

            _userRepository.Update(user);

            bool saved = await _userRepository.SaveChangesAsync(ct);
            if (!saved)
                throw new ActionFailedException(nameof(_userRepository.SaveChangesAsync), "Method failed to execute");

            return new UserUpdateResponseDto() { NewEmail = user.Email, NewPhoneNumber = user.PhoneNumber ?? string.Empty };
        }

        /// <inheritdoc />
        public async Task<bool> UpdateUserPasswordAsync(int id, ChangePasswordRequestDto request, CancellationToken ct)
        {
            var user = await _userRepository.GetFullUserAsync(id, ct);

            if (user == null)
                throw new EntityNotFoundException(nameof(User), $"User with id {id} not found");

            if (user.Identity == null)
            {
                throw new EntityNotFoundException(nameof(UserIdentity), $"User Identity with userId {id} not found");
            }

            if (!PasswordManager.Verify(request.CurrentPassword, user.Identity.PasswordHash))
                return false;

            user.Identity.PasswordHash = PasswordManager.Hash(request.NewPassword);

            _userRepository.Update(user);

            bool saved = await _userRepository.SaveChangesAsync(ct);
            if (!saved)
                throw new ActionFailedException(nameof(_userRepository.SaveChangesAsync), "Method failed to execute");

            return true;
        }
    }
}
