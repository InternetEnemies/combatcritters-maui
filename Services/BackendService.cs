
using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.managers;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;
using CombatCrittersSharp.objects.pack;
using CombatCrittersSharp.objects.user;
using Foundation;

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
        /// Logs in a User with provided credentials
        /// </summary>
        /// <param name="credentials">User login details (username and password)</param>
        /// <returns>A Task representing the login process.</returns>
        public async Task LoginAsync(UserCredentials credentials)
        {
            Console.Write("Attempting to login...");
            await _client.Login(credentials.Username, credentials.Password);
            Console.WriteLine("Login Success!");
        }

        /// <summary>
        /// Create an account using provided credentials
        /// </summary>
        /// <param name="credentials">Account creation details (username and password)</param>
        /// <returns>A task that completes when the account is successfully created</returns>
        public async Task CreateAccountAsync(UserCredentials credentials)
        {
            await ExecuteBackendOperationAsync(async () =>
            {
                Console.WriteLine("Attempting to register user...");
                await _client.Register(credentials.Username, credentials.Password);
                Console.WriteLine("Register user success");
                return Task.CompletedTask;
            }, "User registrationn failed");
        }

        /// <summary>
        /// Get all game Users
        /// </summary>
        /// <returns>A list of all game users</returns>
        public async Task<List<IUser>> GetUsersAsync()
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                Console.WriteLine("Attempting to get all users");

                if (_client.User == null)
                    throw new AuthException("Invalid user");


                var userManager = _client.Users ?? throw new AuthException("User manager is unavailable");
                var users = await userManager.GetAllUsersWithProfiles();

                Console.WriteLine($"Retrieved {users.Count} users");
                return users;

            }, "Failed to fetch users");
        }

        /// <summary>
        /// Delete a User give using ID
        /// </summary>
        /// <param name="id"> user id</param>
        /// <returns>A task that completes when user is deleted</returns>
        /// <exception cref="AuthException">
        /// Thrown if the current user session is invalid or if the user manager is unavailable.
        /// </exception>
        /// <exception cref="RestException">Thrown for network or server issues during deletion.</exception>
        public async Task DeleteUserAsync(int id)
        {
            await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("Invalid user");

                Console.WriteLine("Attempting to delete User");
                var userManager = _client.Users ?? throw new AuthException("User manager is unavailable");

                await userManager.DeleteUser(id);
                Console.WriteLine($"Successfully Removed user {id}");

                return Task.CompletedTask;
            }, "Failed to Remove user");
        }

        /// <summary>
        /// Retrieves all game cards based on a given query filter. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns>A list of card stacks matching the filter criteria, or an empty list if no cards are found.</returns>
        /// <exception cref="AuthException">Thrown when the current user session is invalid or unauthorized.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided card filter query is null or invalid.</exception>
        public async Task<List<IItemStack<ICard>>?> GetCardsAsync(ICardQuery query)
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("Invalid user");

                if (query == null)
                    throw new ArgumentException("Invalid card filter query");

                var cardsManager = _client.User.Cards;
                Console.WriteLine("Attempting to get user cards");
                Console.WriteLine($"User Id is: {_client.User.Id}");

                var cards = await cardsManager.GetCards(query).ConfigureAwait(false);
                Console.WriteLine($"Retrieved {cards.Count} cards");
                return cards;
            }, "Failed to fetch user cards");
        }

        /// <summary>
        /// Retrieves a list of all available packs in the game.
        /// </summary>
        /// <returns>A list of available packs</returns>
        /// <exception cref="AuthException">Thrown if the current user session is invalid or unauthorized.</exception>
        public async Task<List<Pack>?> GetPacksAsync()
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("User session is invalid or unauthorized.");

                var packsManager = _client.User.Packs;
                Console.WriteLine("Attempting to get game packs...");

                var packs = await packsManager.GetAllPacksAsync();
                Console.WriteLine($"Retrieved: {packs?.Count}");
                return packs;
            }, "Failed to fetch packs");
        }

        /// <summary>
        /// Creates a new pack with specified card selections, rarity probabilities, and other pack details.
        /// </summary>
        /// <param name="cardIds">List of card IDs to include as potential contents of the pack.</param>
        /// <param name="rarityProbabilities">Dictionary defining the probability of each rarity level appearing in the pack.</param>
        /// <param name="packName">The name of the pack to be created.</param>
        /// <param name="packImage">The image URL or resource identifier for the pack’s visual representation.</param>
        /// <returns>The created pack if successful.</returns>
        /// <exception cref="AuthException">Thrown if the user session is invalid or unauthorized.</exception>
        /// <exception cref="ArgumentException">Thrown if any argument is invalid, such as an empty list of card IDs.</exception>
        public async Task<IPack> CreatePackAsync(List<int> cardIds, Dictionary<int, int> rarityProbabilities, string packName, string packImage)
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("User session is invalid or unauthorized.");

                if (cardIds == null || cardIds.Count == 0)
                    throw new ArgumentException("At least one card ID must be provided to create a pack.");

                if (string.IsNullOrWhiteSpace(packName))
                    throw new ArgumentException("Pack name cannot be empty.");

                if (string.IsNullOrWhiteSpace(packImage))
                    throw new ArgumentException("Pack image cannot be empty.");

                var packsManager = _client.User.Packs;
                int slotCount = 5;

                Console.WriteLine("Attempting to create pack...");

                var pack = await packsManager.CreatePackAsync(cardIds, rarityProbabilities, packName, packImage, slotCount);

                Console.WriteLine("Success");
                return pack;
            }, "Failed to create packs");
        }


    }
}
