
using Combat_Critters_2._0.Pages;

namespace Combat_Critters_2._0
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			// Set AppShell as the main page for Shell navigation
			MainPage = new NavigationPage(new LoginPage());
		}

		// This method is called after a successful login
		public void NavigateToAppShell()
		{
			// Set AppShell as the main page after login
			MainPage = new AppShell();
		}
	}
}