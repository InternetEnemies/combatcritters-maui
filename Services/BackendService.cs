/*
    The backend service interacts
    with the backend
*/

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
        /// This method gets all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<IUser>> GetUsersAsync()
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                Console.WriteLine("Attempting to get all users");

                if (_client.User == null)
                    throw new AuthException("Invalid user");


                var userManager = _client.Users;
                if (userManager == null)
                    throw new AuthException("User manager is unavailable");

                var users = await userManager.GetAllUsersWithProfiles();

                Console.WriteLine($"Retrieved {users.Count} users");
                return users;

            }, "Failed to fetch users");
        }

        /// <summary>
        /// Request deletion of a user
        /// </summary>
        /// <param name="id"> user id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteUserAsync(int id)
        {
            await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new Exception("Invalid user");

                Console.WriteLine("Attempting to delete User");
                var userManager = _client.Users;

                if (userManager == null)
                    throw new Exception("Invalid user");

                await userManager.DeleteUser(id);
                Console.WriteLine($"Successfully Removed user {id}");


                return Task.CompletedTask;
            }, "Failed to Remove user");
        }
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
        /// This
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<IItemStack<ICard>>?> GetCardsAsync(ICardQuery query)
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new Exception("Invalid user");

                if (query == null)
                    throw new Exception("Invalid card filter query");

                var cardsManager = _client.User.Cards;
                Console.WriteLine("Attempting to get user cards");
                Console.WriteLine($"User Id is: {_client.User.Id}");

                var cards = await cardsManager.GetCards(query).ConfigureAwait(false);
                Console.WriteLine($"Retrieved {cards.Count} cards");
                return cards; //return cards
            }, "Failed to fetch user cards");
        }

        public async Task<IPack> CreatePackAsync(List<int> cardIds, Dictionary<int, int> rarityProbabilities, string packName, string packImage)
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new AuthException("Invalid user");
                var packsManager = _client.User.Packs;

                int slotCount = 5;
                Console.WriteLine("Attempting to create pack");
                var pack = await packsManager.CreatePackAsync(cardIds, rarityProbabilities, packName, packImage, slotCount);
                Console.WriteLine("Success");
                return pack;
            }, "Failed to create packs");
        }

        public async Task<List<Pack>?> GetPacksAsync()
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new Exception("Invalid User");
                var packsManager = _client.User.Packs;

                Console.WriteLine("Attempting to get game packs");
                var packs = await packsManager.GetAllPacksAsync();
                Console.WriteLine($"Retrieved: {packs?.Count}");
                return packs;
            }, "Failed to fetch packs");
        }

        public async Task<List<IDeck>> GetDecksAsync()
        {

            return await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new Exception("Invalid user");

                Console.WriteLine("Attempting to get user decks");
                var deckManager = _client.User.Decks;
                var decks = await deckManager.GetDecks();
                Console.WriteLine($"User has {decks.Count} decks");
                Console.WriteLine("got decks");
                return decks;
            }, "Failed to fetch user decks");
        }

        public async Task FeatureDeckOnProfileAsync(IDeck deck)
        {
            await ExecuteBackendOperationAsync(async () =>
            {
                if (_client.User == null)
                    throw new Exception("Invalid user");

                await _client.User.Profile.SetDeck(deck);

                return Task.CompletedTask;
            }, "Failed to feature the deck on the profile.");
        }

        public async Task<IDeck?> GetFeaturedDeckAsync(IUser user)
        {
            return await ExecuteBackendOperationAsync(async () =>
            {
                if (user == null)
                    throw new Exception("Invalid user");

                var featuredDeck = await user.Profile.GetDeck();

                if (featuredDeck == null)
                    return null; //No Deck has been featured

                return featuredDeck;
            }, "Failed to retrieve the featured deck.");
        }
    }
}
