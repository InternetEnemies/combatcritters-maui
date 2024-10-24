/*
    The backend service interacts
    with the backend
*/

using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;
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
                return await operation();
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
                var cards = await cardsManager.GetCards(query);
                return cards; //return cards
            }, "Failed to fetch user cards");
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
    }
}
