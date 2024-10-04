/*
    This model represents the user's 
    sign up profile information 
*/
namespace Combat_Critters_2._0.Models
{
    public class Profile
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}