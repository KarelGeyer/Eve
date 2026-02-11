using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Users.Domain.Interfaces.Repositories;

namespace Users.Infrastructure
{
    public interface ISessionRepositoryExt : ISessionRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    }
}
