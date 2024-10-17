/*
    The backend service interacts
    with the backend
*/

using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
namespace Combat_Critters_2._0.Services
{
    public static class BackendService
    {
        /// <summary>
        /// This method sends a request to the backend for login
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static async Task<bool> LoginAsync(UserCredentials credentials)
        {
            
            IClient cli = new Client("http://api.combatcritters.ca:4000"); // Create a new client connection
            try
            {
                Console.Write("Attempting to login...");
                await cli.Login(credentials.Username, credentials.Password);
                Console.WriteLine("Login Success...");

                //Store the user client connection
                App.CurrentClient = cli;
                return true;
            }
            catch(RestException e)
            {
                Console.WriteLine($"Failed to Login: {e.Message}");
                return false;
            }        
        }

        public static async Task<bool> CreateAccountAsync(UserCredentials credentials)
        {
            IClient cli = new Client("http://api.combatcritters.ca:4000"); // Create a new client conn
            try
            {
                Console.WriteLine("Attempting to register user...");
                await cli.Register(credentials.Username, credentials.Password);
                Console.WriteLine("Register user success");

                //close the cli connection?
                return true;
            }
            catch(RestException e)
            {
                Console.WriteLine($"Failed to Register user: {e.Message}");
                if (e.ResponseMessage != null)
                    {
                        Console.WriteLine($"Server Response: {e.ResponseMessage.Content}");
                    }
                    if (e.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {e.InnerException.Message}");
                    }          
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred during registration: {ex.Message}");
                if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
                return false;
            }
        }
    }
}
