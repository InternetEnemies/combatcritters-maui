/*
    The backend service interacts
    with the backend
*/

using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;
using Intents;
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
        /// This method sends a request to the backend for login
        /// It throws an exception if login fails.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task LoginAsync(UserCredentials credentials)
        {
            
            try
            {
                Console.Write("Attempting to login...");
                
                await _client.Login(credentials.Username, credentials.Password);
                Console.WriteLine("Login Success...");
                
            }
            catch(RestException e)
            {
                Console.WriteLine($"Failed to Login: {e.Message}");
                
                throw; //throw the exception
            }        
        }

        public async Task CreateAccountAsync(UserCredentials credentials)
        {
            try
            {
                Console.WriteLine("Attempting to register user...");
                await _client.Register(credentials.Username, credentials.Password);
                Console.WriteLine("Register user success");
            }
            catch(RestException e)
            {
                Console.WriteLine($"Failed to Register user: {e.Message}");

                throw; //throw the exception
            }
        }

        /// <summary>
        /// This
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<IItemStack<ICard>>?> GetCardsAsync(ICardQuery query)
        {
            //Fetch cards if they are not caches yet
            try
            {
                if(_client.User != null)
                {
                    if(query != null) //query must not be null
                    {
                        var cardsManager = _client.User.Cards;
                        Console.WriteLine("Attempting to get user cards");
                        var cards = await cardsManager.GetCards(query);
                        return cards; //return cards
                    }
                    else
                    {
                        throw new Exception("Invalid card filter query");
                    }
                }
                else
                {
                    throw new Exception("Invalid user");
                }
            }
            catch (RestException)
            {
                throw; // throw the exception
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<IDeck>?> GetDecksAsync()
        {
            try
            {
                if (_client.User != null)
                {
                    var deckManager = _client.User.Decks;
                    Console.WriteLine("Attempting to get user decks");
                    var decks = await deckManager.GetDecks();
                    Console.WriteLine("got decks");
                    return decks;
                }
                else
                {
                    Console.Write("User is null");
                    return null;
                }
            }
            catch (RestException e)
            {
                Console.WriteLine($"Faild to get user cards {e.Message}");
                throw; // throw the exception
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
