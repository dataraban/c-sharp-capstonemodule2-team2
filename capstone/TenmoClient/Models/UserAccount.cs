

namespace TenmoClient.Models
{
    public class UserAccount 
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }

    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
