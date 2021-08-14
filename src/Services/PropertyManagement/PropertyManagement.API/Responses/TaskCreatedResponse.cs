using System;

namespace PropertyManagement.API.Responses
{
    public class TaskCreatedResponse
    {
        public TaskCreatedResponse(Guid taskId)
        {
            TaskId = taskId;
        }
        public string Message { get; set; }
        public Guid TaskId { get; }
    }
}
