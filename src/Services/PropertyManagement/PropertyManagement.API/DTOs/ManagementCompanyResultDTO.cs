using System.Text.Json.Serialization;

namespace PropertyManagement.API.DTOs
{
    public class ManagementCompanyResultDTO
    {
        [JsonPropertyName("mgmt")]
        public ManagementCompanyDTO ManagementCompany { get; set; }
    }
}
