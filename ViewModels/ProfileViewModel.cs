using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.objects.deck;
using CombatCrittersSharp.objects.user;

namespace Combat_Critters_2._0.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private IDeck _featuredDeck;
        private bool _hasFeaturedDeck; //boolean for content triggers

        private ObservableCollection<IUser> _users;
        private bool _hasUsers; //boolean for content triggers

        private IUser _selectedUser;


        public IDeck FeaturedDeck
        {
            get => _featuredDeck;
            set
            {
                _featuredDeck = value;
                OnPropertyChanged(nameof(FeaturedDeck));
            }
        }
        public bool HasFeaturedDeck
        {
            get => _hasFeaturedDeck;
            set
            {
                _hasFeaturedDeck = value;
                OnPropertyChanged(nameof(HasFeaturedDeck));
            }
        }

        public ObservableCollection<IUser> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
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

        public IUser SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ProfileViewModel()
        {

            //_featuredDeck = new IDeck();
            _hasFeaturedDeck = false;
            _users = new ObservableCollection<IUser>();
            _hasUsers = false;
            //_selectedUser = new User();

            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
