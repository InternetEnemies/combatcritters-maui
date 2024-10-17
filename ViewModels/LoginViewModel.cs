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

public class LoginViewModel : INotifyPropertyChanged
{
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
    }

    private async void OnLogin()
    {
        try
        {
            var loginRequestToBackend = await BackendService.LoginAsync(new UserCredentials{
                Username = Username,
                Password = Password
            });
            
            if (loginRequestToBackend)
            {
                //The client con has been stored in backendservices

                //Navigate to User Profile page
                (Application.Current as App)?.NavigateToAppShell();
            }
            else
            {
                
                //show an error message
                //Maybe display a notification in UI
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to login: {ex.Message}");
            
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