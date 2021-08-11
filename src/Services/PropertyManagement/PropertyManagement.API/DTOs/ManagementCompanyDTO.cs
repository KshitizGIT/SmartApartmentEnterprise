using System.Text.Json.Serialization;

namespace PropertyManagement.API.DTOs
{
    public class ManagementCompanyDTO
    {
        [JsonPropertyName("mgmtID")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("market")]
        public string Market { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
