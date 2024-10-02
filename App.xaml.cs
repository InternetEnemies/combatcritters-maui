using Combat_Critters_2._0.Pages;

namespace Combat_Critters_2._0
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			// Set the LoginPage as the first page 
			MainPage = new NavigationPage(new LoginPage());
		}

		//This method is called after a successful login
		public void NavigateToDashboard()
		{
			//This would set the AppShell as the main page
			MainPage = new AppShell();
		}
	}

}

