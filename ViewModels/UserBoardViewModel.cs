
using System.Collections.ObjectModel;
using System.ComponentModel;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.user;

namespace Combat_Critters_2._0.ViewModels
{
    public class UserBoardViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private ObservableCollection<IUser> _allUsers;
        private bool _hasUsers;
        private ObservableCollection<IUser> _filteredUser;

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
            HasUsers = false;
            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
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
            try
            {
                var users = await _backendService.GetUsersAsync(); //get all users from backendf

                if (users != null && users.Count > 0)
                {
                    Application.Current?.Dispatcher.Dispatch(() =>
                    {
                        AllUsers = new ObservableCollection<IUser>(users);
                        HasUsers = true;
                    });
                }
                else
                {
                    //Game has no Users
                    HasUsers = false;
                    AllUsers.Clear();
                }

            }
            catch (RestException)
            {
                HasUsers = false;

                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load users. Please try again.", "OK");
            }
            catch (Exception)
            {
                HasUsers = false;
                throw; //bubble up to the global exception
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