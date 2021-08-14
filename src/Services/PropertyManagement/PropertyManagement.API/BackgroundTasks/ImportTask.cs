﻿using PropertyManagement.Infrastructure.Models;
using System;
using System.Threading.Tasks;
using TaskStatus = PropertyManagement.Infrastructure.Models.TaskStatus;

namespace PropertyManagement.API.BackgroundTasks
{
    public abstract class ImportTask
    {
        public TaskDetails TaskDetails { get; private set; }
        public ImportTask(string filePath) : this(Guid.NewGuid(), filePath)
        {
        }
        public ImportTask(Guid id, string filePath)
        {
            Id = id;
            FilePath = filePath;
            InitializeTaskDetail();
        }

        public Guid Id { get; }
        public string FilePath { get; }
        public async Task RunAsync(ImportTaskContext context)
        {
            //Reload task details as runasync executes in a different thread than it was created on.
            TaskDetails = await context.DbContext.TaskDetails.FindAsync(TaskDetails.Id);

            try
            {
                await MarkAsInProgress(context);

                await ConcreteRunAsync(context);

                await MarkAsSuccess(context);
            }
            catch (Exception ex)
            {
                await FlagAsFailure(context, ex.Message);
            }

        }


        private void InitializeTaskDetail()
        {
            TaskDetails = new TaskDetails()
            {
                Created = DateTime.Now,
                Id = Id,
                Status = TaskStatus.Pending,
                Type = this.GetType().FullName,
                Updated = DateTime.Now
            };

        }

        public abstract Task ConcreteRunAsync(ImportTaskContext context);

        private async Task FlagAsFailure(ImportTaskContext context, string reason)
        {
            TaskDetails.Message = reason;
            TaskDetails.Status = TaskStatus.Failure;
            TaskDetails.Updated = DateTime.Now;
            await context.DbContext.SaveChangesAsync();
        }
        private async Task MarkAsSuccess(ImportTaskContext context)
        {
            TaskDetails.Message = "Import task completed successfully.";
            TaskDetails.Status = TaskStatus.Success;
            TaskDetails.Updated = DateTime.Now;
            await context.DbContext.SaveChangesAsync();
        }

        private async Task MarkAsInProgress(ImportTaskContext context)
        {
            TaskDetails.Status = TaskStatus.Progress;
            TaskDetails.Updated = DateTime.Now;
            await context.DbContext.SaveChangesAsync();
        }

        private async Task MarkAsPending(ImportTaskContext context)
        {
            TaskDetails.Status = TaskStatus.Pending;
            TaskDetails.Updated = DateTime.Now;
            await context.DbContext.SaveChangesAsync();
        }
    }
}
