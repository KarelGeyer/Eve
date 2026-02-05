using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Users.Domain.Interfaces.Reposiroties
{
    public interface IUserRepository
    {
        #region Base CRUD Operations
        Task<User?> GetAsync(int id, CancellationToken ct);
        User? Add(User user);
        bool Update(User user);
        bool Delete(User user);
        #endregion

        #region Extended CRUD Operations
        Task<User?> GetFullUserAsync(int id, CancellationToken ct);
        Task<bool> DoesUserWithEmailExist(string email, CancellationToken ct);
        Task<bool> DoesUserWithUsernameExist(string username, CancellationToken ct);
        Task<UserIdentity?> GetUserIdentityAsync(int id, CancellationToken ct);
        Task<UserSettings?> GetUserSettingsAsync(int id, CancellationToken ct);
        #endregion

        Task<bool> SaveChangesAsync(CancellationToken ct);
    }
}
