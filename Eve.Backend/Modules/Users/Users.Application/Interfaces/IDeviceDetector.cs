using System;
using System.Collections.Generic;
using System.Text;
using Common.Shared.Models;

namespace Users.Application.Interfaces
{
    public interface IDeviceDetector
    {
        Device GetDeviceType();
    }
}
