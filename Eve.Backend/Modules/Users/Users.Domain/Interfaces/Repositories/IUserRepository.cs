using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Users.Domain.Interfaces.Reposiroties
{
    /// <summary>
    /// Defines a contract for user repository operations, providing methods for creating, retrieving, updating, and
    /// deleting user entities, as well as accessing extended user information and settings asynchronously.
    /// </summary>
    /// <remarks>Implementations of this interface are responsible for managing user data persistence and
    /// retrieval, including support for cancellation via tokens in asynchronous operations. Methods allow for checking
    /// user existence by email or username, retrieving user profiles and settings, and activating user accounts. Thread
    /// safety and transactional guarantees depend on the specific implementation.</remarks>
    public interface IUserRepository
    {
        #region Base CRUD Operations
        /// <summary>
        /// Retrieves a Base user entity by its unique identifier.
        /// This method is designed to fetch only the core user information without any related entities or additional details.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<User?> GetAsync(int id, CancellationToken ct);

        /// <summary>
        /// Adds a new user to the db.
        /// </summary>
        /// <param name="user">The user to add</param>
        /// <returns>The added user instance if the operation succeeds; otherwise, null if the user could not be added.</returns>
        User? Add(User user);

        /// <summary>
        /// Updates the specified user record with new information.
        /// </summary>
        /// <param name="user">The user object containing updated information. Cannot be null.</param>
        /// <returns>true if the user was successfully updated; otherwise, false.</returns>
        bool Update(User user);

        /// <summary>
        /// Deletes the specified user from the system.
        /// </summary>
        /// <param name="user">The user to be deleted. Cannot be null.</param>
        /// <returns>true if the user was successfully deleted; otherwise, false.</returns>
        bool Delete(User user);
        #endregion

        #region Extended CRUD Operations
        /// <summary>
        /// Retrieves the complete user record, including all associated details, for the specified user ID
        /// asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <param name="ct">A cancellation token</param>
        /// <returns>The task result contains the full user record if found; otherwise, null.</returns>
        Task<User?> GetFullUserAsync(int id, CancellationToken ct);

        /// <summary>
        /// Asynchronously retrieves the full user profile associated with the specified email address.
        /// </summary>
        /// <remarks>If no user exists with the specified email address, the method returns null.</remarks>
        /// <param name="email">The email address of the user to retrieve. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token</param>
        /// <returns>The task result contains the full user profile if found; otherwise, null.</returns>
        Task<User?> GetFullUserAsync(string email, CancellationToken ct);

        /// <summary>
        /// Determines whether a user with the specified email address exists.
        /// </summary>
        /// <param name="email">The email address to check for an existing user. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token</param>
        /// <returns>The task result is <see langword="true"/> if a user with
        /// the specified email exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> DoesUserWithEmailExist(string email, CancellationToken ct);

        /// <summary>
        /// Determines whether a user with the specified username exists.
        /// </summary>
        /// <param name="username">The username to search for. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result is <see langword="true"/> if a user with
        /// the specified username exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> DoesUserWithUsernameExist(string username, CancellationToken ct);

        /// <summary>
        /// Asynchronously retrieves the user identity associated with the specified user ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose identity is to be retrieved.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user identity if found;
        /// otherwise, <see langword="null"/>.</returns>
        Task<UserIdentity?> GetUserIdentityAsync(int id, CancellationToken ct);

        /// <summary>
        /// Asynchronously retrieves the user settings for the specified user identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose settings are to be retrieved. Must be a positive integer.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user settings for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        Task<UserSettings?> GetUserSettingsAsync(int id, CancellationToken ct);

        /// <summary>
        /// Asynchronously retrieves the user settings associated with the specified authentication token.
        /// </summary>
        /// <param name="token">The authentication token used to identify the user. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user settings if the token
        /// is valid; otherwise, null.</returns>
        Task<UserSettings?> GetUserSettingsByTokenAsync(string token, CancellationToken ct);

        /// <summary>
        /// Activates a user account asynchronously using the specified activation token.
        /// </summary>
        /// <param name="token">The activation token associated with the user account to be activated. Cannot be null or empty.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the number of user accounts
        /// activated. Returns 0 if no account was activated.</returns>
        Task<int> ActivateUserAsync(string token, CancellationToken ct);
        #endregion

        /// <summary>
        /// Asynchronously saves all pending changes to the data store.
        /// </summary>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result is <see langword="true"/> if changes
        /// were successfully saved; otherwise, <see langword="false"/>.</returns>
        Task<bool> SaveChangesAsync(CancellationToken ct);
    }
}
