using PropertyManagement.Infrastructure.Models;

namespace PropertyManagement.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static SearchResult ToSearchResult(this Property property)
        {

            return new SearchResult()
            {
                Type = nameof(Property),
                Id = $"{nameof(Property)}_{property.Id}",
                PropertyId = property.Id,
                Market = property.Market,
                City = property.City,
                FormerName = property.FormerName,
                Name = property.Name,
                State = property.State,
                StreetAddress = property.StreetAddress,
                Latitude = property.Latitude,
                Longitude = property.Longitude
            };
        }

        public static SearchResult ToSearchResult(this ManagementCompany company)
        {
            return new SearchResult()
            {
                Type = nameof(ManagementCompany),
                Id = $"{nameof(ManagementCompany)}_{company.Id}",
                ManagementCompanyId = company.Id,
                Market = company.Market,
                Name = company.Name,
                State = company.State
            };
        }

    }
}
