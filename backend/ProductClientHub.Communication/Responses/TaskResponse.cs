namespace ProductClientHub.Communication.Responses
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedToClient { get; set; } 
        public DateTime? DueDate { get; set; }
    }
}
