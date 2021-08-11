using System.Text.Json.Serialization;

namespace PropertyManagement.API.DTOs
{
    public class PropertyResultDTO
    {
        [JsonPropertyName("property")]
        public PropertyDTO Property { get; set; }
    }
}
