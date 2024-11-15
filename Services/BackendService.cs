/*
    The backend service interacts
    with the backend
*/

using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.managers.Implementation;
using CombatCrittersSharp.managers.interfaces;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CombatCrittersSharp.objects.pack;
using CombatCrittersSharp.objects.user;
namespace Combat_Critters_2._0.Services
{
    public class BackendService(IClient client)
    {
        private readonly IClient _client = client;

        /// <summary>
        /// Wraps backend operation with safety net to handle errors gracefully. 
        /// Ensuring exceptions are caught, logged and rethrown when needed
        /// </summary>
        /// <typeparam name="T">The type of result expected from the backend operation.</typeparam>
        /// <param name="operation">The Operationto be performed</param>
        /// <param name="errorMessage">A custom message to provide context in case of an error.</param>
        /// <returns>The result of the backend operation, if successful.</returns>
        /// 
        /// 
        /// <exception cref="UnauthorizedAccessException">Thrown when an authorization error occurs.</exception>
        /// <exception cref="HttpRequestException">Thrown for REST-related issues with HTTP details.</exception>
        /// <exception cref="Exception">Thrown for any other unexpected errors, with a general message.</exception>
        /// <exception cref="TimeoutException">Thrown for operations that exceed a time limit.</exception>
        /// <exception cref="ArgumentException">Thrown for invalid arguments passed to backend calls.</exception>
        private static async Task<T> ExecuteBackendOperationAsync<T>(Func<Task<T>> operation, string errorMessage)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (AuthException ex)
            {
                // Handle authorization errors
                Console.WriteLine($"{errorMessage}: Authorization Error - {ex.Message}");
                Console.WriteLine($"Detailed Message: {ex.DetailedMessage}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                throw new UnauthorizedAccessException($"{errorMessage}: {ex.Message}", ex);
            }
            catch (RestException ex)
            {
                // Handle REST-related errors with HTTP details
                Console.WriteLine($"{errorMessage}: REST Error - {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                Console.WriteLine($"Response Content: {ex.ResponseContent ?? "No response content provided"}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");


                throw new HttpRequestException($"{errorMessage}: {ex.Message} (Status Code: {ex.StatusCode})", ex);
            }
            catch (CombatCrittersException ex)
            {
                // Handle any other CombatCritters-specific errors
                Console.WriteLine($"{errorMessage}: Combat Critters Error - {ex.Message}");
                Console.WriteLine($"Detailed Message: {ex.DetailedMessage}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                throw new Exception($"{errorMessage}: {ex.Message}", ex);
            }
            catch (TimeoutException ex)
            {
                // Handle timeout errors explicitly
                Console.WriteLine($"{errorMessage}: Operation Timed Out - {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                throw new TimeoutException($"{errorMessage}: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                // Handle argument errors
                Console.WriteLine($"{errorMessage}: Argument Error - {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");


                throw new ArgumentException($"{errorMessage}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                Console.WriteLine($"{errorMessage}: Unexpected Error - {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                throw new Exception($"{errorMessage}: An unexpected error occurred.", ex);
            }
        }

        /// <summary>
        /// Logs in a User with provided credentials
        /// </summary>
        /// <param name="credentials">User login details (username and password)</param>
        /// <returns>A Task representing the login process.</returns>
        public async Task LoginAsync(UserCredentials credentials)
        {
            await ExecuteBackendOperationAsync(async () =>
            {
                Console.Write("Attempting to login...");
                await _client.Login(credentials.Username, credentials.Password);
                Console.WriteLine("Login Success!");
                return Task.CompletedTask;
            }, "Login failed");
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
        public async Task<IPack?> CreatePackAsync(List<int> cardIds, Dictionary<int, int> rarityProbabilities, string packName, string packImage)
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

        /// <summary>
        /// Get all vendors in game.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AuthException"></exception>
        public async Task<List<Vendor>> GetVendorsAsync()
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("User session is invalid or unauthorized.");

                var marketPlaceManager = _client.User.MarketPlace;
                var vendors = await marketPlaceManager.GetVendorsAsync();
                return vendors;
            }, "Failed to return game vendors");
        }

        public async Task<Offer?> GetVendorOfferAsync(int id)
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("User session is invalid or unathorized");

                var marketplaceManager = _client.User.MarketPlace;
                var offer = await marketplaceManager.GetVendorOfferAsync(id);
                return offer;
            }, "Failed to return vendor offer");
        }

        //debug
        public async Task<string> GetAndLogVendorOfferAsync(int vendorId)
        {
            try
            {
                // Get the raw JSON response from the wrapper method
                var json = await _client.User.MarketPlace.GetVendorOfferJsonAsync(vendorId);
                //Console.WriteLine("Raw JSON Response: " + json); // Log the JSON structure for inspection

                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }



    }
}
