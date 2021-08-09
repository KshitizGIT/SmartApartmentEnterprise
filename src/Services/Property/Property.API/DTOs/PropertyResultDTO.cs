using System.Text.Json.Serialization;

namespace Property.API.DTOs
{
    public class PropertyResultDTO
    {
        [JsonPropertyName("property")]
        public PropertyDTO Property { get; set; }
    }
}
