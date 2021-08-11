using PropertyManagement.API.DTOs;
using PropertyManagement.API.Events;
using PropertyManagement.API.Extensions;
using PropertyManagement.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PropertyManagement.API.BackgroundTasks
{
    public class ImportPropertyTask : ImportTask
    {
        public ImportPropertyTask(string filePath) : base(id: Guid.NewGuid(), filePath)
        {

        }

        public async override Task ConcreteRunAsync(ImportTaskContext context)
        {
            using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var properties = await JsonSerializer.DeserializeAsync<List<PropertyResultDTO>>(stream);

            var records = properties
                .DistinctBy(p => p.Property.Id) // Make sure to only use distinct records
                .Select(s => s.ToProperty()).ToList();

            //Fetching all the property ids in the file to query existing dataset.
            var ids = records.Select(s => s.Id).ToList();

            // This dictionary will contain all distinct properties that imported from file. Its value will
            // denote whether it is a new property or existing one.
            // Set all records to be non existent at first.
            Dictionary<Property, bool> propertyExistenceHash = records.ToDictionary(s => s, d => false);

            // Get properties in file that are already persisted in the database
            // These properties might need to be updated in case their attribute value changes
            var existingRecords = context.DbContext.Properties.Where(p => ids.Any(r => r.Equals(p.Id)));

            foreach (var record in existingRecords)
            {
                //Update the existing records with the values from file.
                var newProperty = records.First(r => r.Id.Equals(record.Id));
                UpdateExisitingProperty(newProperty, record);

                // Set properties as already existed.
                propertyExistenceHash[newProperty] = true;
            }

            // Fetch properties that should be removed.
            var recordsToDelete = context.DbContext.Properties.Where(p => !ids.Any(r => r.Equals(p.Id))).ToList();
            context.DbContext.Properties.RemoveRange(recordsToDelete);

            // New properties that should be persisted.
            var recordsToCreate = propertyExistenceHash.Where(s => !s.Value).Select(r => r.Key).ToList();
            await context.DbContext.Properties.AddRangeAsync(recordsToCreate);

            List<Models.Property> updatedEntries = new List<Models.Property>();

            if (context.DbContext.ChangeTracker.HasChanges())
            {
                // Fetch changed entries from ChangeTracker
                updatedEntries = context.DbContext.ChangeTracker.Entries<Models.Property>()
                   .Where(entry => entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                   .Select(s => s.Entity)
                   .ToList();
            }
            await context.DbContext.SaveChangesAsync();

            // Raise PropertiesCreatedEvent 
            if (recordsToCreate != null && recordsToCreate.Count > 0)
            {
                await context.Mediator.Send(new PropertiesCreatedEvent(recordsToCreate));
            }
            //Raise PropertiesUpdatedEvent
            if (updatedEntries != null && updatedEntries.Count > 0)
            {
                await context.Mediator.Send(new PropertiesUpdatedEvent(updatedEntries));
            }
            if (recordsToDelete != null && recordsToDelete.Count > 0)
            {
                //Raise PropertiesDeletedEvent
                await context.Mediator.Send(new PropertiesDeletedEvent(recordsToDelete));
            }
        }
        private void UpdateExisitingProperty(Property newProperty, Property existingProperty)
        {
            existingProperty.Market = newProperty.Market;
            existingProperty.City = newProperty.City;
            existingProperty.FormerName = newProperty.FormerName;
            existingProperty.Name = newProperty.Name;
            existingProperty.State = newProperty.State;
            existingProperty.StreetAddress = newProperty.StreetAddress;
            existingProperty.Latitude = newProperty.Latitude;
            existingProperty.Longitude = newProperty.Longitude;
        }
    }
}
