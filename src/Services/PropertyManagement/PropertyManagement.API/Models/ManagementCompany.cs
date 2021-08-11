using PropertyManagementAPI.Models;

namespace PropertyManagement.API.Models
{
    public class ManagementCompany : MarketEntity
    {
        public string Name { get; set; }
        public string State { get; set; }
    }
}
