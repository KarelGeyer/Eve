using Common.Shared.Models;

namespace Users.Application.Interfaces
{
    public interface IDeviceDetector
    {
        Device GetDeviceType();
    }
}
