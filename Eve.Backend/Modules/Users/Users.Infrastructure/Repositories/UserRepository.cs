using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Interfaces.Reposiroties;
using Users.Infrastructure;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Provides methods for accessing and managing user data and related entities in the underlying data store.
    /// </summary>
    /// <remarks>The UserRepository offers asynchronous and synchronous operations for retrieving, adding,
    /// updating, and deleting user records, as well as handling user settings and identities. It is designed to be used
    /// within application services or controllers to encapsulate data access logic for user-related functionality.
    /// Thread safety depends on the underlying UserContext implementation; typically, repositories are not thread-safe
    /// and should be scoped per request in web applications.</remarks>
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<User?> GetAsync(int id, CancellationToken ct)
        {
            var user = await _context.User.FindAsync(id, ct);

            if (user == null)
                return null;

            return user;
        }

        /// <inheritdoc/>
        public async Task<User?> GetFullUserAsync(int id, CancellationToken ct)
        {
            var user = await _context
                .User.Where(u => u.Id == id)
                .Include(u => u.Settings)
                .Include(u => u.Identity)
                .FirstOrDefaultAsync(ct);

            if (user == null)
                return null;

            return user;
        }

        /// <inheritdoc/>
        public async Task<User?> GetFullUserAsync(string email, CancellationToken ct)
        {
            var user = await _context
                .User.Where(u => u.Email == email)
                .Include(u => u.Settings)
                .Include(u => u.Identity)
                .FirstOrDefaultAsync(ct);

            if (user == null)
                return null;

            return user;
        }

        /// <inheritdoc/>
        public async Task<bool> DoesUserWithEmailExist(string email, CancellationToken ct)
        {
            var user = await _context.User.Where(u => u.Email == email).FirstAsync(ct);
            return user != null;
        }

        /// <inheritdoc/>
        public async Task<bool> DoesUserWithUsernameExist(string username, CancellationToken ct)
        {
            var user = await _context.User.Where(u => u.Username == username).FirstAsync(ct);
            return user != null;
        }

        /// <inheritdoc/>
        public async Task<UserIdentity?> GetUserIdentityAsync(int id, CancellationToken ct)
        {
            var identity = await _context.UserIdentity.Where(ui => ui.UserId == id).FirstOrDefaultAsync(ct);

            if (identity == null)
                return null;

            return identity;
        }

        /// <inheritdoc/>
        public async Task<UserSettings?> GetUserSettingsAsync(int id, CancellationToken ct)
        {
            var settings = await _context.UserSettings.Where(ui => ui.UserId == id).FirstOrDefaultAsync(ct);

            if (settings == null)
                return null;

            return settings;
        }

        /// <inheritdoc/>
        public async Task<UserSettings?> GetUserSettingsByTokenAsync(string token, CancellationToken ct)
        {
            var settings = await _context.UserSettings.Where(s => s.ActivationToken == token).FirstOrDefaultAsync();

            if (settings == null)
                return null;

            return settings;
        }

        /// <inheritdoc/>
        public bool Update(User user)
        {
            if (user == null)
                return false;

            var result = _context.User.Update(user);

            return result != null;
        }

        /// <inheritdoc/>
        public bool Delete(User user)
        {
            if (user == null)
                return false;

            var response = _context.User.Remove(user);
            return response != null;
        }

        /// <inheritdoc/>
        public User? Add(User user)
        {
            if (user == null)
                return null;

            var response = _context.User.Add(user);
            return response.Entity;
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChangesAsync(CancellationToken ct)
        {
            int result = await _context.SaveChangesAsync(ct);
            return result > 0;
        }

        /// <inheritdoc/>
        public async Task<int> ActivateUserAsync(string token, CancellationToken ct)
        {
            return await _context
                .UserSettings.Where(s => s.ActivationToken == token && s.ActivationTokenExpiration > DateTime.UtcNow)
                .ExecuteUpdateAsync(
                    setters =>
                        setters
                            .SetProperty(s => s.IsActive, true)
                            .SetProperty(s => s.IsEmailVerified, true)
                            .SetProperty(s => s.ActivationToken, (string?)null)
                            .SetProperty(s => s.ActivationTokenExpiration, (DateTime?)null),
                    ct
                );
        }
    }
}
