using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Interfaces.Reposiroties;
using Users.Infrastructure;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(int id, CancellationToken ct)
        {
            var user = await _context.User.FindAsync(id, ct);

            if (user == null)
                return null;

            return user;
        }

        public async Task<User?> GetFullUserAsync(int id, CancellationToken ct)
        {
            var user = await _context.User.Where(u => u.Id == id).Include(u => u.Settings).Include(u => u.Identity).FirstOrDefaultAsync(ct);

            if (user == null)
                return null;

            return user;
        }

        public async Task<bool> DoesUserWithEmailExist(string email, CancellationToken ct)
        {
            var user = await _context.User.Where(u => u.Email == email).FirstAsync(ct);
            return user != null;
        }

        public async Task<bool> DoesUserWithUsernameExist(string username, CancellationToken ct)
        {
            var user = await _context.User.Where(u => u.Username == username).FirstAsync(ct);
            return user != null;
        }

        public async Task<UserIdentity?> GetUserIdentityAsync(int id, CancellationToken ct)
        {
            var identity = await _context.UserIdentity.Where(ui => ui.UserId == id).FirstOrDefaultAsync(ct);

            if (identity == null)
                return null;

            return identity;
        }

        public async Task<UserSettings?> GetUserSettingsAsync(int id, CancellationToken ct)
        {
            var settings = await _context.UserSettings.Where(ui => ui.UserId == id).FirstOrDefaultAsync(ct);

            if (settings == null)
                return null;

            return settings;
        }

        public bool Update(User user)
        {
            if (user == null)
                return false;

            var result = _context.User.Update(user);

            return result != null;
        }

        public bool Delete(User user)
        {
            if (user == null)
                return false;

            var response = _context.User.Remove(user);
            return response != null;
        }

        public User? Add(User user)
        {
            if (user == null)
                return null;

            var response = _context.User.Add(user);
            return response.Entity;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken ct)
        {
            int result = await _context.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}
