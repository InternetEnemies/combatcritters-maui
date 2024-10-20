using Combat_Critters_2._0.Pages;


namespace Combat_Critters_2._0
{
	public partial class App : Application
	{

		public App()
		{

			InitializeComponent();

			// Set the Loginpage as the initial page
			MainPage = new NavigationPage(new LoginPage());
		}

		// This method is called after a successful login
		public void NavigateToAppShell()
		{
			// Set AppShell as the main page after login
			MainPage = new AppShell();
		}

		// This method is called when the user logs out
		public void NavigateToLoginPage()
		{
			//Set LoginPage as the main page for logout
			MainPage = new NavigationPage(new LoginPage());
		}
	}
}