namespace Combat_Critters_2._0;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Set the LoginPage as the first page 
		MainPage = new NavigationPage(new LoginPage());
	}
}
