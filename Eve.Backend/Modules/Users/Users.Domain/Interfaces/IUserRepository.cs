using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entites;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
    }
}
