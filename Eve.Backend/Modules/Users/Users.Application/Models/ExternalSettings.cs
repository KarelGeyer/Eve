namespace Users.Application.Models
{
    /// <summary>
    /// Externí nastavení z Program.cs nebo appsettings.json.
    /// </summary>
    public class ExternalSettings
    {
        public string AbstractApiKey { get; set; } = string.Empty;

        /// <summary>
        /// URL pro validaci emailů pomocí Abstract API.
        /// </summary>
        public string AbstractApiUrl { get; set; } = string.Empty;
    }
}
