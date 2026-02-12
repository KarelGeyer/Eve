using System.Text.Json.Serialization;

namespace Users.Application.Dtos
{
    public class AbstractEmailResponseDto
    {
        [JsonPropertyName("email_deliverability")]
        public DeliverabilityDetails Deliverability { get; set; } = new();

        [JsonPropertyName("email_quality")]
        public EmailQualityDetails Quality { get; set; } = new();

        public class DeliverabilityDetails
        {
            [JsonPropertyName("status_detail")]
            public string StatusDetail { get; set; } = string.Empty;
        }

        public class EmailQualityDetails
        {
            [JsonPropertyName("is_disposable")]
            public bool IsDisposable { get; set; }

            [JsonPropertyName("is_live_site")]
            public bool IsLiveSite { get; set; }
        }
    }
}
