
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.user;
using CombatCrittersSharp.objects.userpack;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace Combat_Critters_2._0.ViewModels
{
    public class UserBoardViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private ObservableCollection<IUser> _allUsers;
        private bool _hasUsers;
        private ObservableCollection<IUser> _filteredUser;
        private ObservableCollection<ICard> _selectedUserProfileDeckCards;
        private IUser? _selectedUser;

        public ICommand DeleteUserCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public IUser? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));

                LoadSelectedUserCards();
                LoadSelectedUserProfileDeckCards();
                LoadSelectedUserPacks();
            }
        }

        public ObservableCollection<ICard> SelectedUserProfileDeckCards
        {
            get => _selectedUserProfileDeckCards;
            set
            {
                _selectedUserProfileDeckCards = value;
                OnPropertyChanged(nameof(SelectedUserProfileDeckCards));
            }
        }

        public bool HasUsers
        {
            get => _hasUsers;
            set
            {
                _hasUsers = value;
                OnPropertyChanged(nameof(HasUsers));
            }
        }
        public ObservableCollection<IUser> AllUsers
        {
            get => _allUsers;
            set
            {
                _allUsers = value;
                OnPropertyChanged(nameof(AllUsers));
            }
        }

        public ObservableCollection<IUser> FilteredUser
        {
            get => _filteredUser;
            set
            {
                _filteredUser = value;
                OnPropertyChanged(nameof(FilteredUser));
            }
        }

        private ObservableCollection<ICard> _selectedUserCards;
        public ObservableCollection<ICard> SelectedUserCards
        {
            get => _selectedUserCards;
            set
            {
                _selectedUserCards = value;
                OnPropertyChanged(nameof(SelectedUserCards));
            }
        }

        private ObservableCollection<UserPack> _selectedUserPacks;
        public ObservableCollection<UserPack> SelectedUserPacks
        {
            get => _selectedUserPacks;
            set
            {
                _selectedUserPacks = value;
                OnPropertyChanged(nameof(SelectedUserPacks));
            }
        }


        public UserBoardViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _allUsers = new ObservableCollection<IUser>();
            _filteredUser = new ObservableCollection<IUser>();

            _selectedUserProfileDeckCards = new ObservableCollection<ICard>();
            _selectedUserCards = new ObservableCollection<ICard>();
            _selectedUserPacks = new ObservableCollection<UserPack>();
            HasUsers = false;


            DeleteUserCommand = new Command<IUser>(DeleteUser);
            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        /// <summary>
        /// Delete a selected user
        /// </summary>
        /// <param name="user"></param>
        private async void DeleteUser(IUser user)
        {
            if (user != null)
            {
                try
                {
                    await _backendService.DeleteUserAsync(user.Id);

                    //Reload User List
                    await LoadUsers();

                    var toast = Toast.Make($"{user.Username} has been removed from Combat Critters", ToastDuration.Short);
                    await toast.Show();
                }
                catch (RestException e)
                {
                    //Log
                    Console.WriteLine(e.Message);

                    //Rest Exception
                    var toast = Toast.Make("System Error", ToastDuration.Short);
                    await toast.Show();

                }
                catch (AuthException e)
                {
                    //Log
                    Console.WriteLine(e.Message);

                    //Auth Exception
                    var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                    await toast.Show();
                }
                catch (InvalidOperationException e)
                {
                    //Log
                    Console.WriteLine(e.Message);

                    var toast = Toast.Make(e.Message, ToastDuration.Short);
                    await toast.Show();
                }
            }
            else
            {
                var toast = Toast.Make($"{user} does not exist", ToastDuration.Short);
                await toast.Show();
            }
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadUsers();
        }

        /// <summary>
        /// Load all game users 
        /// </summary>
        /// <returns></returns>
        private async Task LoadUsers()
        {
            IsLoading = true;
            bool hasUsers = false; //function scoped variable
            try
            {
                var users = await _backendService.GetUsersAsync(); //get all users from backendf

                if (users != null && users.Count > 0)
                {
                    AllUsers = new ObservableCollection<IUser>(users);
                    FilteredUser = new ObservableCollection<IUser>(users);
                    hasUsers = true;
                }
                else
                {
                    //Game has no Users
                    AllUsers.Clear();
                }

            }
            catch (InvalidOperationException e)
            {
                //Log
                Console.WriteLine(e.Message);

                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }
            catch (RestException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }

            finally
            {
                HasUsers = hasUsers;
                IsLoading = false; //Turn off loading indicator
            }

        }

        private async void LoadSelectedUserPacks()
        {
            IsLoading = true;
            try
            {
                if (SelectedUser != null)
                    SelectedUserPacks = await _backendService.GetUserPacksAsync(SelectedUser.Id);
            }
            catch (RestException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }

            finally
            {
                IsLoading = false;
            }
        }
        private async void LoadSelectedUserCards()
        {
            IsLoading = true;
            try
            {
                var query = new CardQueryBuilder();
                query.SetOwned(true);
                SelectedUserCards = await _backendService.GetCardsAsync(query.Build());
            }
            catch (RestException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }

            finally
            {
                IsLoading = false;
            }


        }
        /// <summary>
        /// Load the user profile deck cards when a user a selected
        /// </summary>
        private async void LoadSelectedUserProfileDeckCards()
        {
            if (SelectedUser?.ProfileDeck != null)
            {
                IsLoading = true;
                try
                {
                    Console.WriteLine("Getting user profile deck...");
                    var cards = await SelectedUser.ProfileDeck.GetCards();
                    Console.WriteLine($"{cards.Count} cards on profile");
                    SelectedUserProfileDeckCards = new ObservableCollection<ICard>(cards);
                    Console.WriteLine("UI Updated");
                }
                catch (RestException e)
                {
                    //Log
                    Console.WriteLine(e.Message);

                    //Rest Exception
                    var toast = Toast.Make("System Error", ToastDuration.Short);
                    await toast.Show();

                }
                catch (AuthException e)
                {
                    //Log
                    Console.WriteLine(e.Message);

                    //Auth Exception
                    var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                    await toast.Show();
                }

                finally
                {
                    IsLoading = false;
                }
            }
            else
            {
                SelectedUserProfileDeckCards.Clear();
            }
        }

        /// <summary>
        /// Filter users based on search text
        /// </summary>
        /// <param name="searchText"></param>
        public void FilterUsers(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                // Show all users if search text is empty
                FilteredUser = new ObservableCollection<IUser>(AllUsers);
            }
            else
            {
                // Filter users based on the search text
                var filtered = AllUsers.Where(u => u.Username.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                FilteredUser = new ObservableCollection<IUser>(filtered);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}