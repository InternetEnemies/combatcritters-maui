namespace Combat_Critters_2._0;


public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    // Login button click handler
    private void OnLoginClicked(object sender, EventArgs eventArgs)
    {
        //What happens when a user tries to login
        string userName = username.Text;
        string userPassword = password.Text;

        //Do something.
    }

    // Create Account button Click handler
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateAccountPage());
    }
}