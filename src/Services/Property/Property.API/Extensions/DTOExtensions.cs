using Property.API.DTOs;
using Property.Models;

namespace Property.API.Extensions
{
    public static class DTOExtensions
    {
        public static SearchResult ToSearchResult(this PropertyResultDTO dto)
        {

            return new SearchResult()
            {
                Type = nameof(Property),
                EntityId = dto.Property.Id,
                Market = dto.Property.Market,
                City = dto.Property.City,
                FormerName = dto.Property.FormerName,
                Name = dto.Property.Name,
                State = dto.Property.State,
                StreetAddress = dto.Property.StreetAddress,
                Latitude = dto.Property.Latitude,
                Longitude = dto.Property.Longitude
            };
        }

        public static SearchResult ToSearchResult(this ManagementCompanyResultDTO dto)
        {

            return new SearchResult()
            {
                Type = nameof(ManagementCompany),
                EntityId = dto.ManagementCompany.Id,
                Market = dto.ManagementCompany.Market,
                Name = dto.ManagementCompany.Name,
                State = dto.ManagementCompany.State
            };
        }
    }
}
