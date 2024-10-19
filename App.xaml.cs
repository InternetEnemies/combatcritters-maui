
using System.Security.Cryptography.X509Certificates;
using Combat_Critters_2._0.Pages;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp;
using CombatCrittersSharp.objects.card;

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
		public async void NavigateToAppShell()
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