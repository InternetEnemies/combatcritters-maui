/*
    This handles login logic 
    and communicates with the backend
    to authenticate the user.
*/
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0;
using Combat_Critters_2._0.Models;
using Combat_Critters_2._0.Pages;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;

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

            // On success, Navigate to User Profile page
            (Application.Current as App)?.NavigateToAppShell();

        }
        catch (RestException)
        {
            //UI feedback on failed login
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Incorrect username or password. Please try again.", "OK");
        }
        catch (Exception)
        {
            throw;
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