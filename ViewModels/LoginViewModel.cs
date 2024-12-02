/*
    This handles login logic 
    and communicates with the backend
    to authenticate the user.
*/
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;
using Combat_Critters_2._0.Pages;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;

namespace Combat_Critters_2._0.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private string _username = "";
        private string _password = "";
        private readonly INavigation _navigation; //For navigation to Create account

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        //Command for handling login
        public ICommand LoginCommand { get; }

        // Command for navigating to the Create Account page
        public ICommand CreateAccountCommand { get; }


        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            LoginCommand = new Command(OnLogin);
            CreateAccountCommand = new Command(OnCreateAccount);

            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
        }

        /// <summary>
        /// Send request for login 
        /// </summary>
        private async void OnLogin()
        {
            try
            {
                // Call the backend service for login
                await _backendService.LoginAsync(new UserCredentials
                {
                    Username = Username,
                    Password = Password
                });

                // On success, Navigate to Hero
                (Application.Current as App)?.NavigateToHeroPage(Username);

            }
            catch (AuthException)
            {
                //Handle authorization errors
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Authorization error. Please check your username and password and try again.", "OK");
            }
            catch (RestException)
            {
                //Handle Rest specific errors
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "A network or server error occurred. Please try again.", "OK");
            }
            catch (TimeoutException)
            {
                //Handle timeout specific error
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "The login request timed out. Please check your network and try again.", "OK");
            }
            catch (Exception)
            {
                // Handle any other unexpected errors
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "An unexpected error occurred. Please try again.", "OK");
            }


        }

        private async void OnCreateAccount()
        {
            // Navigate to the CreateAccountPage
            await _navigation.PushAsync(new CreateAccountPage());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
