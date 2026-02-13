using Users.Domain.Entities;

namespace Users.Domain.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        /// <summary>
        /// Asynchronously retrieves the user sessions for the specified user identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose sessions are to be retrieved. Must be a positive integer.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user sessions for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        Task<List<UserSession>?> GetUserSessionsAsync(int id, CancellationToken ct);

        /// <summary>
        /// Asynchronously retrieves the user sessions for the specified user identifier that are marked as active.
        /// </summary>
        /// <param name="id">The unique identifier of the user whose sessions are to be retrieved. Must be a positive integer.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user sessions for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        Task<List<UserSession>?> GetActiveUserSessionsAsync(int id, CancellationToken ct);

        /// <summary>
        /// Asynchronously retrieves the user sessions for the specified device.
        /// </summary>
        /// <param name="device">A device that is to be found. Must not be null</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user session for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        Task<UserSession?> GetUserSessionAsync(Guid id, CancellationToken ct);

        /// <summary>
        /// Creates a new user session for the specified device.
        /// </summary>
        /// <param name="newSession">A new session entity to be added. Must not be null</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user session for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        UserSession? Add(UserSession newSession);

        /// <summary>
        /// Updates a current user session for the specified device.
        /// </summary>
        /// <param name="sessionToUpdate">A session entity to be updated. Must not be null</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user session for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        bool Update(UserSession sessionToUpdate);

        /// <summary>
        /// Deletes the user session.
        /// </summary>
        /// <param name="session">A <see cref="UserSession"/> id. Must not be null</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result contains the user session for the
        /// specified user, or <see langword="null"/> if no settings are found.</returns>
        bool Delete(UserSession session);

        /// <summary>
        /// Asynchronously saves all pending changes to the data store.
        /// </summary>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>The task result is <see langword="true"/> if changes
        /// were successfully saved; otherwise, <see langword="false"/>.</returns>
        Task<bool> SaveChangesAsync(CancellationToken ct);
    }
}
