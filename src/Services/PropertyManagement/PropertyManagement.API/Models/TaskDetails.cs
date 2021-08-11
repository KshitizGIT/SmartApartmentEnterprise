using System;

namespace PropertyManagement.API.Models
{
    public class TaskDetails
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public TaskStatus Status { get; set; }
        public string Message { get; set; }
    }
}
