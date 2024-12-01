
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.managers;
using CombatCrittersSharp.managers.interfaces;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;
using CombatCrittersSharp.objects.pack;
using CombatCrittersSharp.objects.user;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
        /// Request Login
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
            Console.WriteLine("Attempting to get all users");
            IUserManager? userManager = _client.Users;

            // if for some reason, this client does not have connection to the User manager
            if (userManager == null)
            {
                throw new InvalidOperationException("This client has no access to the User Manager");
            }

            var users = await userManager.GetAllUsersWithProfiles();
            Console.WriteLine($"Retrieved {users.Count} users");
            return users;
        }

        /// <summary>
        /// Request delete a user
        /// </summary>
        /// <param name="id"> user id</param>
        /// <returns>A task that completes when user is deleted</returns>
        public async Task DeleteUserAsync(int id)
        {
            Console.WriteLine("Attempting to delete User");
            IUserManager? userManager = _client.Users;

            // if for some reason, this client does not have connection to the User manager
            if (userManager == null)
            {
                throw new InvalidOperationException("This client has no access to the User Manager");
            }

            await userManager.DeleteUser(id);
            Console.WriteLine($"Successfully Removed user {id}");
        }


        /// <summary>
        /// Request for all game cards
        /// </summary>
        /// <param name="query"> the card query filter</param>
        /// <returns>Returns an observable collection of cards</returns>
        /// <exception cref="InvalidOperationException">returned if User and/or cards manager is null</exception>
        public async Task<ObservableCollection<ICard>?> GetCardsAsync(ICardQuery query)
        {

            try
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
            catch (InvalidOperationException)
            {
                //If this happens, either client instance is null of user instance of client is null
                //Display popup
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();

            }
            catch (ArgumentNullException)
            {
                //If this happens, the argument for card Query is null
                var toast = Toast.Make("Invalid Card Query", ToastDuration.Short);
                await toast.Show();

            }
            catch (RestException)
            {
                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException)
            {
                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }
            // Return null in the case of an exception.
            return null;


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
        // public async Task<IPack> CreatePackAsync(List<int> cardIds, Dictionary<int, int> rarityProbabilities, string packName, string packImage)
        // {

        //    // var pack = await packsManager.CreatePackAsync(cardIds, rarityProbabilities, packName, packImage, slotCount);

        //     Console.WriteLine("Success");

        //     return pack;
        // }


    }
}
