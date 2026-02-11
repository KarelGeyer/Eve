using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Shared.Exceptions
{
    public class AccountLockedException : Exception
    {
        public int UserId { get; }

        public AccountLockedException(int userId)
            : base($"Account with id '{userId}' is blocked.")
        {
            UserId = userId;
        }
    }
}
