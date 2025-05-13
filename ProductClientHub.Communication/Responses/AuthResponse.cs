namespace ProductClientHub.Communication.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
<<<<<<< Updated upstream

        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
=======
        public Guid UserId { get; set; }
>>>>>>> Stashed changes
        public string Email { get; set; } = string.Empty;
    }
}
