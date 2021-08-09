namespace Property.Models
{
    public class Property : MarketEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}
