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
    public class BackendService
    {
        private readonly IClient _client;
        public BackendService(IClient client)
        {
            _client = client; //Client is injected here
        }

        /// <summary>
        /// This is a local exception handler for backend services
        /// This is used for logging and to improve readability on methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<T> ExecuteBackendOperationAsync<T>(Func<Task<T>> operation, string errorMessage)
        {
            try
            {
                //Excute the operation
                return await operation().ConfigureAwait(false);
            }
            catch (RestException ex)
            {
                //Log specific RestException
                Console.WriteLine($"{errorMessage}", ex);
                throw new Exception(errorMessage, ex);
            }
            catch (Exception ex)
            {
                //Catch general errors and throw them up the stack
                Console.WriteLine($"{errorMessage}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// This method sends a request to the backend for login
        /// It throws an exception if login fails.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task LoginAsync(UserCredentials credentials)
        {
            await ExecuteBackendOperationAsync(async () =>
            {
                Console.Write("Attempting to login...");
                await _client.Login(credentials.Username, credentials.Password);
                Console.WriteLine("Login Success...");
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
                    throw new Exception("Invalid user");


                var userManager = _client.Users;
                if (userManager == null)
                    throw new Exception("Invalid user");

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
