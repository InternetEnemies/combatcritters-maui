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
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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
            catch (AuthException e)
            {
                //Log
                Console.WriteLine(e.Message);

                var toast = Toast.Make("Invalid User", ToastDuration.Short, 14);
                await toast.Show();
            }
            catch (RestException e)
            {
                //Log
                Console.WriteLine(e.Message);

                var toast = Toast.Make("An Erro Occured. Please Try again", ToastDuration.Short, 14);
                await toast.Show();
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
