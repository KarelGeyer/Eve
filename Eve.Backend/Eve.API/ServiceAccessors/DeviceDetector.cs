using Common.Shared.Models;
using Users.Application.Interfaces;
using Wangkanai.Detection.Services;

namespace Eve.API.ServiceAccessors
{
    public class DeviceDetector(IDetectionService detectionService) : IDeviceDetector
    {
        public Device GetDeviceType()
        {
            try
            {
                var type = detectionService.Device.Type.ToString();
                var platform = detectionService.Platform.Name.ToString();
                return new Device { Type = type, Platform = platform };
            }
            catch
            {
                return new();
            }
        }
    }
}
