
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.user;
using CommunityToolkit.Maui.Alerts;


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

                //Load the selected user's profile deck cards
                LoadSelectedUserProfileDeckCards();
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

        public UserBoardViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _allUsers = new ObservableCollection<IUser>();
            _filteredUser = new ObservableCollection<IUser>();
            _selectedUserProfileDeckCards = new ObservableCollection<ICard>();
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

                    var toast = Toast.Make($"{user.Username} has been removed from Combat Critters", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                }
                catch (RestException)
                {
                    if (Application.Current?.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to Delete user. Please try again.", "OK");
                }
            }
            else
            {
                var toast = Toast.Make($"{user} does not exist", CommunityToolkit.Maui.Core.ToastDuration.Short);
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
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load users. Please try again.", "OK");
            }

            finally
            {
                // Set HasUsers.
                HasUsers = hasUsers;
                IsLoading = false; //Turn off loading indicator
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
                catch (RestException)
                {
                    if (Application.Current?.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to loading users profile. Please try again.", "OK");
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