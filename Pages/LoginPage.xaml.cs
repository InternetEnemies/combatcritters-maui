using Microsoft.Maui.Controls;

namespace Combat_Critters_2._0.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // Login button click handler
        private async void OnLoginClicked(object sender, EventArgs eventArgs)
        {
            //What happens when a user tries to login
            string userName = username.Text;
            string userPassword = password.Text;

            //Do something.

            bool loginSuccess = true;

            if (loginSuccess)
            {
                //Navigate to dashboard
                (Application.Current as App).NavigateToDashboard();
            }
            else
            {
                //Show Login error
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }

        // Create Account button Click handler
        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountPage());
        }
    }
}


