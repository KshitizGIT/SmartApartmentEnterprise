using System.Text.Json.Serialization;

namespace PropertyManagement.API.DTOs
{
    public class PropertyDTO
    {
        [JsonPropertyName("propertyID")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("formerName")]
        public string FormerName { get; set; }
        [JsonPropertyName("streetAddress")]
        public string StreetAddress { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("market")]
        public string Market { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("lat")]
        public float Latitude { get; set; }

        [JsonPropertyName("lng")]
        public float Longitude { get; set; }
    }
}
