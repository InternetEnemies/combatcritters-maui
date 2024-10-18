/*
    The backend service interacts
    with the backend
*/

using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
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
    }
}
