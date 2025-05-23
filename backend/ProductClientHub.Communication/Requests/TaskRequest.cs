namespace ProductClientHub.Communication.Requests
{
    public class TaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid? AssignedTo { get; set; }
    } 
}
