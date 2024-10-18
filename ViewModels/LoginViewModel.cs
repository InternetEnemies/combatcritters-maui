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
            await _backendService.LoginAsync(new UserCredentials{
                Username = Username,
                Password = Password
            });
            
            //Navigate to User Profile page
            (Application.Current as App)?.NavigateToAppShell();
            
        }
        catch(RestException ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");

            //Display a UI alert for a login failure
            if (Application.Current?.MainPage !=  null)
                await Application.Current.MainPage.DisplayAlert("Login Failed", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");

            //display a notification in UI
            if (Application.Current?.MainPage !=  null)
                await Application.Current.MainPage.DisplayAlert("Error", "An unepected error occured. Please try again", "OK");
            
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