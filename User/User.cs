namespace EgenInlämning
{
    public class User
    {
        public Guid Id { get; init; }

        public double Balance { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
