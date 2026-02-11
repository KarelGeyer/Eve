using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Interfaces
{
    public interface IUserContextService
    {
        string GetIpAddress();
        string GetUserAgent();
    }
}
