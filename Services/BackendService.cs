
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.managers.interfaces;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CombatCrittersSharp.objects.pack;
using CombatCrittersSharp.objects.user;
using CombatCrittersSharp.objects.userpack;
using CombatCrittersSharp.rest.payloads;
namespace Combat_Critters_2._0.Services
{

    /// <summary>
    /// This is a Service class that uses the CombatCritterSharp wrapper to make 
    /// API calls to the backend.
    /// 
    /// Exceptions will be handles by the callers of any method in this class. For UI Operations. 
    /// </summary>
    /// <param name="client">The client connection to the API</param>
    public class BackendService(IClient client)
    {
        private readonly IClient _client = client;

        /// <summary>
        /// Request Login
        /// </summary>
        /// <param name="credentials">User login details (username and password)</param>
        /// <returns>A Task representing the login process.</returns>
        public async Task LoginAsync(UserCredentials credentials)
        {
            Console.WriteLine("Attempting to login...");
            await _client.Login(credentials.Username, credentials.Password);
            Console.WriteLine("Login Success!");
        }

        /// <summary>
        /// Request create account
        /// </summary>
        /// <param name="credentials">Account creation details (username and password)</param>
        /// <returns>A task that completes when the account is successfully created</returns>
        public async Task CreateAccountAsync(UserCredentials credentials)
        {
            Console.WriteLine("Attempting to register user...");
            await _client.Register(credentials.Username, credentials.Password);
            Console.WriteLine("Register user success");
        }

        /// <summary>
        /// Request Get all Users
        /// </summary>
        /// <returns>A list of all game users</returns>
        public async Task<List<IUser>> GetUsersAsync()
        {

            IUserManager? userManager = _client.Users;

            if (userManager != null)
            {
                Console.WriteLine("Attempting to get all users");
                var users = await userManager.GetAllUsersWithProfiles();
                Console.WriteLine($"Retrieved {users.Count} users");
                return users;
            }
            else
                throw new InvalidOperationException("This client has no access to the User Manager");

        }

        /// <summary>
        /// Request delete a user
        /// </summary>
        /// <param name="id"> user id</param>
        /// <returns>A task that completes when user is deleted</returns>
        public async Task DeleteUserAsync(int id)
        {
            IUserManager? userManager = _client.Users;
            if (userManager != null)
            {
                Console.WriteLine("Attempting to delete User");
                await userManager.DeleteUser(id);
                Console.WriteLine($"Successfully Removed user {id}");
            }
            else
                throw new InvalidOperationException("This client has no access to the User Manager");

        }


        /// <summary>
        /// Request for all game cards
        /// </summary>
        /// <param name="query"> the card query filter</param>
        /// <returns>Returns an observable collection of cards</returns>
        /// <exception cref="InvalidOperationException">returned if User and/or cards manager is null</exception>
        public async Task<ObservableCollection<ICard>> GetCardsAsync(ICardQuery query)
        {

            IUser? user = _client.User;

            if (user != null && user.Cards != null)
            {
                IUserCardsManager cardsManager = user.Cards;
                Console.WriteLine("Attempting to get user cards");
                var cards = await cardsManager.GetCards(query).ConfigureAwait(false);
                Console.WriteLine($"Retrieved {cards.Count} cards");

                //cards can return an empty list or a populated list
                var modifiedCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                return modifiedCards;
            }
            else
                throw new InvalidOperationException("This client User cannot be null");

        }

        /// <summary>
        /// Request for all game packs
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">returned if User and/or Packmanager is null</exception>
        public async Task<List<Pack>?> GetPacksAsync()
        {

            IUser? user = _client.User;

            if (user != null && user.Packs != null)
            {
                IPackManager packsManager = user.Packs;
                Console.WriteLine("Attempting to get game packs...");
                var packs = await packsManager.GetAllPacksAsync();
                Console.WriteLine($"Retrieved: {packs?.Count}");
                return packs;
            }
            else
                throw new InvalidOperationException("This Client user cannot be null");

        }

        public async Task<ObservableCollection<UserPack>> GetUserPacksAsync(int id)
        {
            IUser? user = _client.User;

            if (user != null && user.Packs != null)
            {
                IPackManager packsManager = user.Packs;
                List<UserPack> packs = await packsManager.GetUserPacksAsync(id);
                return new ObservableCollection<UserPack>(packs);
            }
            else
                throw new InvalidOperationException("This Client user cannot be null");
        }
        /// <summary>
        /// Request for a pack to be created
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <param name="cardIds"></param>
        /// <param name="slotRaritiesProbabilities"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Pack?> CreatePackAsync(string name, string image, int[] cardIds, List<Dictionary<int, int>> slotRaritiesProbabilities)
        {
            IUser? user = _client.User;
            if (user != null && user.Packs != null)
            {
                IPackManager packsManager = user.Packs;
                Console.WriteLine($"Attempting to create pack '{name}'");
                var pack = await packsManager.CreatePackAsync(name, image, cardIds, slotRaritiesProbabilities);
                Console.WriteLine("Pack Creation Task Completed");
                return pack;
            }
            else
                throw new InvalidOperationException("This Client user cannot be null");

        }

        /// <summary>
        /// Request for all game vendors
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ObservableCollection<Vendor>> GetVendorsAsync()
        {
            IUser? user = _client.User;
            if (user != null && user.MarketPlace != null)
            {
                IMarketPlaceManager marketPlaceManager = user.MarketPlace;
                Console.WriteLine("Attempting to get all vendord");
                var vendors = await marketPlaceManager.GetVendorsAsync();

                return new ObservableCollection<Vendor>(vendors);
            }
            else
                throw new InvalidOperationException("This Client user cannot be null");
        }

        /// <summary>
        /// Request for a list of vendor offers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Offer>> GetVendorOfferAsync(int id)
        {
            IUser? user = _client.User;
            if (user != null && user.MarketPlace != null)
            {
                IMarketPlaceManager marketPlaceManager = user.MarketPlace;
                Console.WriteLine("Attempting to get Vendor Offer");
                var vendorOffers = await marketPlaceManager.GetVendorOfferAsync(id);

                return vendorOffers;

            }
            else
                throw new InvalidOperationException("This Client user cannot be null");
        }

        public async Task<Offer?> CreateNewVendorOfferAsync(int vendorId, int newLevel, List<OfferCreationItem> collectItems, OfferCreationItem giveItem)
        {
            IUser? user = _client.User;
            if (user != null && user.MarketPlace != null)
            {
                IMarketPlaceManager marketPlaceManager = user.MarketPlace;
                Console.WriteLine($"Attempting to create new Offer Level {newLevel}");
                var createdOffer = await marketPlaceManager.CreateOfferAsync(vendorId, newLevel, collectItems, giveItem);
                if (createdOffer == null)
                    Console.WriteLine("Offer Creation Failed");
                else
                    Console.WriteLine("Offer Creation Success");
                return createdOffer;

            }
            else
                throw new InvalidOperationException("This Client user cannot be null");
        }

        // //debug
        // public async Task<string> GetAndLogVendorOfferAsync(int vendorId)
        // {
        //     try
        //     {
        //         // Get the raw JSON response from the wrapper method
        //         var json = await _client.User.MarketPlace.GetVendorOfferJsonAsync(vendorId);
        //         //Console.WriteLine("Raw JSON Response: " + json); // Log the JSON structure for inspection

        //         return json;
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine("An error occurred: " + ex.Message);
        //         return null;
        //     }
        // }



    }
}
