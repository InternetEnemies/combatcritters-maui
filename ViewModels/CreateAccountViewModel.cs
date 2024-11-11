/*
    This handles the account creation logic
    and communicates with the backend to create a new user account
*/
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;

public class CreateAccountViewModel : INotifyPropertyChanged
{
    private readonly BackendService _backendService;

    private string _username = "";
    private string _password = "";
    private readonly INavigation _navigation;
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

    public ICommand CreateAccountCommand { get; } //Command for handling Create Account

    public CreateAccountViewModel(INavigation navigation)
    {
        _navigation = navigation;
        CreateAccountCommand = new Command(OnCreateAccount);

        _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
    }

    /// <summary>
    /// Handle Account Creation
    /// </summary>
    private async void OnCreateAccount()
    {
        try
        {
            // Call the backend services to create a new account
            await _backendService.CreateAccountAsync(new UserCredentials
            {
                Username = Username,
                Password = Password
            });

            // On success Navigate back to Login page
            await _navigation.PopAsync();

        }
        catch (AuthException)
        {
            // Authorization-specific message
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Authorization Error", "You do not have permission to create an account. Please check with support.", "OK");
        }
        catch (RestException)
        {
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Register Failed", "Failed to register. Please check your credentials and try again.", "OK");
        }
        catch (Exception)
        {
            // General catch-all for unexpected issues
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Registration Failed", "An unexpected error occurred. Please try again later.", "OK");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}