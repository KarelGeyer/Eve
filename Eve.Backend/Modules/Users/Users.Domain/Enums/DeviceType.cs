namespace Users.Domain.Enums
{
    /// <summary>
    /// Specifies the supported device types for platform-specific operations.
    /// </summary>
    /// <remarks>Use this enumeration to indicate the target device platform when performing actions that
    /// depend on the operating system, such as configuring platform-specific settings or handling device
    /// compatibility.</remarks>
    public enum DeviceType
    {
        Android = 1,
        iOS = 2,
        Web = 3,
        Tv = 4,
        SmartWatch = 5,
    }
}
