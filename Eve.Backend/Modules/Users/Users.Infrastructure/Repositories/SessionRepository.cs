using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Interfaces.Repositories;

namespace Users.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly UserContext _context;

        public SessionRepository(UserContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<UserSession>?> GetUserSessionsAsync(int id, CancellationToken ct)
        {
            var sessions = await _context.UserSessions.Where(s => s.UserId == id).ToListAsync(ct);

            if (sessions == null)
                return null;

            return sessions;
        }

        /// <inheritdoc/>
        public async Task<List<UserSession>?> GetActiveUserSessionsAsync(int id, CancellationToken ct)
        {
            var sessions = await _context.UserSessions.Where(s => s.UserId == id && s.IsActive).ToListAsync(ct);

            if (sessions == null)
                return null;

            return sessions;
        }

        /// <inheritdoc/>
        public async Task<UserSession?> GetUserSessionAsync(Guid uuid, CancellationToken ct)
        {
            var session = await _context.UserSessions.Where(s => s.SessionId == uuid).FirstOrDefaultAsync(ct);

            if (session == null)
                return null;

            return session;
        }

        /// <inheritdoc/>
        public UserSession? Add(UserSession newSession)
        {
            if (newSession == null)
                return null;

            var response = _context.UserSessions.Add(newSession);
            return response.Entity;
        }

        /// <inheritdoc/>
        public bool Delete(UserSession session)
        {
            if (session == null)
                return false;

            var response = _context.UserSessions.Remove(session);
            return response != null;
        }

        /// <inheritdoc/>
        public bool Update(UserSession sessionToUpdate)
        {
            if (sessionToUpdate == null)
                return false;

            var result = _context.UserSessions.Update(sessionToUpdate);

            return result != null;
        }

        /// <inheritdoc/>
        public async Task<bool> SaveChangesAsync(CancellationToken ct)
        {
            int result = await _context.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}
