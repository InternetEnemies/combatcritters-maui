/*
    The backend service interacts
    with the backend
*/

using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception; //The wrapper class
public static class BackendService
{
    public static async Task<bool> LoginAsync(UserCredentials credentials)
    {

        var client = new Client("http://api.combatcritters.ca:4000");
        try
        {
            Console.Write("Attempting to login...");
            await client.Login(credentials.Username, credentials.Password);
            Console.WriteLine("Login Success...");
            return true;
        }
        catch(RestException e)
        {
            Console.WriteLine($"Failed to Login: {e.Message}");
            return false;
        }        
    }

    public static async Task<bool> CreateAccountAsync(Profile credentials)
    {
        // Interact with the Java backend to create a new account
        //return await WrapperClient.CreateAccount(credentials);
        return true;
    }

    //Fetch the user's owned cards
    public static async Task<List<Card>> GetUserCardsAsync()
    {
        //Interact with the backend using the wrapper
        //... 

        //For testing
        return new List<Card>
        {
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
            new() {CardId = 1, Name = "UglyMan, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"}

        };
    }

    //Fetch User Decks
    public static async Task<List<Deck>> GetUserDecksAsync()
    {
        //Interact with the backend using the wrapper
        //...

        //For testing 
        return new List<Deck>
        {
             new()
             {
                Name = "Test Deck 1",
                    Cards = new List<Card>
                    {
                        new Card{CardId = 1, Name = "UglyMan1, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
                        new Card{CardId = 1, Name = "UglyMan1, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
                        new Card{CardId = 1, Name = "UglyMan1, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
                        new Card{CardId = 1, Name = "UglyMan1, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
                        new Card{CardId = 1, Name = "UglyMan1, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"}
                    }
             },
             new()
             {
                Name = "Test Deck 2",
                    Cards = new List<Card>
                    {
                        new Card{CardId = 1, Name = "UglyMan2, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
                        new Card{CardId = 1, Name = "UglyMan2, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"}
                    }
             },
             new()
             {
                Name = "Test Deck 3",
                    Cards = new List<Card>
                    {
                        new Card{CardId = 1, Name = "UglyMan3, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"},
                        new Card{CardId = 1, Name = "UglyMan3, the hedious hero", PlayCost=5, Rarity = 2, Image = "testimage.jpeg", Type="Critter",Description="This is test card 1"}
                    }
             }
        };
    }
}