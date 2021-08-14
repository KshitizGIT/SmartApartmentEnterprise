using PropertyManagement.API.DTOs;
using PropertyManagement.Infrastructure.Models;

namespace PropertyManagement.API.Extensions
{

    /// <summary>
    /// Helper extension class that facilates mapping between and from DTOs
    /// </summary>
    public static class MappingExtensions
    {
        public static ManagementCompany ToManagementCompany(this ManagementCompanyResultDTO dto)
        {
            return new ManagementCompany()
            {
                Id = dto.ManagementCompany.Id,
                Market = dto.ManagementCompany.Market.Trim(),
                Name = dto.ManagementCompany.Name.Trim(),
                State = dto.ManagementCompany.State.Trim()
            };
        }
        public static Property ToProperty(this PropertyResultDTO dto)
        {
            return new Property()
            {
                Id = dto.Property.Id,
                Market = dto.Property.Market.Trim(),
                City = dto.Property.City.Trim(),
                FormerName = dto.Property.FormerName.Trim(),
                Name = dto.Property.Name.Trim(),
                State = dto.Property.State.Trim(),
                StreetAddress = dto.Property.StreetAddress.Trim(),
                Latitude = dto.Property.Latitude,
                Longitude = dto.Property.Longitude
            };
        }
    }
}
