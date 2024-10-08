/*
    This handles the account creation logic
    and communicates with the backend to create a new user account
*/
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;
using CombatCrittersSharp;
using CombatCrittersSharp.exception;

public class CreateAccountViewModel : INotifyPropertyChanged
{
    private string _firstName = "";
    private string _lastName = "";
    private string _email = "";
    private string _username = "";
    private string _password = "";
    private readonly INavigation _navigation;
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value; //Update the backing field with the new value
            OnPropertyChanged(nameof(FirstName)); //Notify the UI that the property has changed
        }
    }
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value; //Update the backing field with the new value
            OnPropertyChanged(nameof(LastName)); //Notify the UI that the property has changed
        }
    }
    public string Email
    {
        get => _email;
        set
        {
            _email = value; //Update the backing field with the new value
            OnPropertyChanged(nameof(Email)); //Notify the UI that the property has changed
        }
    }
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
    //Command for handling Create Account
    public ICommand CreateAccountCommand { get; }

    public CreateAccountViewModel(INavigation navigation)
    {
        _navigation = navigation;
        CreateAccountCommand = new Command(OnCreateAccount);
    }

    private async void OnCreateAccount()
    {
        
        var result = await BackendService.CreateAccountAsync(new Profile
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Username = Username,
            Password = Password
        });

        //TEST
        result = true;
        if (result)
        {
            // Navigate back to the login page
            await _navigation.PopAsync();
        }


    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}