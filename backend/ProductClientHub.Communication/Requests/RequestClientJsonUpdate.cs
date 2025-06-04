namespace ProductClientHub.Communication.Requests
{
    public class RequestClientJsonUpdate
    {
        public Guid Id { get; set; }
        public string NewName { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
        public string NewDescription { get; set; } = string.Empty;
    }
}
