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
    public class ImportManagementCompanyTask : ImportTask
    {
        public ImportManagementCompanyTask(string filePath) : base(Guid.NewGuid(), filePath)
        {

        }
        public async override Task ConcreteRunAsync(ImportTaskContext context)
        {
            using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var mgCompanies = await JsonSerializer.DeserializeAsync<List<ManagementCompanyResultDTO>>(stream);

            var records = mgCompanies
                .DistinctBy(p => p.ManagementCompany.Id) // Make sure to only use distinct records
                .Select(s => s.ToManagementCompany()).ToList();

            //Fetching all the managementCompany ids in the file to query existing dataset.
            var ids = records.Select(s => s.Id).ToList();

            // This dictionary will contain all distinct management companies that imported from file. Its value will
            // denote whether it is a new record or existing one. Set all records to be non existent at first.
            Dictionary<ManagementCompany, bool> mgExistenceHash = records.ToDictionary(s => s, d => false);

            // Get management companies in file that are already persisted in the database
            // These might need to be updated in case their attribute value changes
            var existingRecords = context.DbContext.ManagementCompanies.Where(p => ids.Any(r => r.Equals(p.Id)));

            foreach (var record in existingRecords)
            {
                //Update the existing records with the values from file.
                var newManagementCompany = records.First(r => r.Id.Equals(record.Id));
                UpdateExistingManagementCompany(newManagementCompany, record);

                // Set properties as already existed.
                mgExistenceHash[newManagementCompany] = true;
            }

            // Fetch records that should be removed.
            var recordsToDelete = context.DbContext.ManagementCompanies.Where(p => !ids.Any(r => r.Equals(p.Id))).ToList();
            context.DbContext.ManagementCompanies.RemoveRange(recordsToDelete);

            // New companies that should be persisted.
            var recordsToCreate = mgExistenceHash.Where(s => !s.Value).Select(r => r.Key).ToList();

            await context.DbContext.ManagementCompanies.AddRangeAsync(recordsToCreate);

            List<ManagementCompany> updatedEntries = new List<ManagementCompany>();

            if (context.DbContext.ChangeTracker.HasChanges())
            {
                updatedEntries = context.DbContext.ChangeTracker.Entries<ManagementCompany>()
                    .Where(entry => entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    .Select(e => e.Entity)
                    .ToList();

            }
            await context.DbContext.SaveChangesAsync();

            if (recordsToCreate != null && recordsToCreate.Count > 0)
            {
                //Fire ManagementCompaniesCreatedEvent
                await context.Mediator.Send(new ManagementCompaniesCreatedEvent(recordsToCreate));
            }
            //Fire ManagementCompaniesDeletedEvent
            if (updatedEntries != null && updatedEntries.Count > 0)
            {
                await context.Mediator.Send(new ManagementCompaniesUpdatedEvent(updatedEntries));
            }
            if (recordsToDelete != null && recordsToDelete.Count > 0)
            {
                //Fire ManagementCompaniesDeletedEvent
                await context.Mediator.Send(new ManagementCompaniesDeletedEvent(recordsToDelete));
            }
        }
        private void UpdateExistingManagementCompany(ManagementCompany newCompany, ManagementCompany existing)
        {
            existing.Market = newCompany.Market;
            existing.Name = newCompany.Name;
            existing.State = newCompany.State;
        }
    }
}
