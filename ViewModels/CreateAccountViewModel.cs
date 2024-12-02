/*
    This handles the account creation logic
    and communicates with the backend to create a new user account
*/
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}