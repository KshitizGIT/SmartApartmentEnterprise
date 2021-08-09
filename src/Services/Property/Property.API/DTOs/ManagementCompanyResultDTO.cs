using System.Text.Json.Serialization;

namespace Property.API.DTOs
{
    public class ManagementCompanyResultDTO
    {
        [JsonPropertyName("mgmt")]
        public ManagementCompanyDTO ManagementCompany { get; set; }
    }
}
