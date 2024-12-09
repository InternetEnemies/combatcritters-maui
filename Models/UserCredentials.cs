/*
    This model represents the user's 
    credentials during login or account creation
*/
namespace Combat_Critters_2._0.Models
{
    public class UserCredentials 
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
