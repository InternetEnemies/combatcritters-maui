using Combat_Critters_2._0.Pages;

namespace Combat_Critters_2._0
{
	public partial class App : Application
	{

		public App()
		{

			InitializeComponent();

			// Global exvceptio handling
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


			// Set the Loginpage as the initial page
			MainPage = new NavigationPage(new LoginPage());
		}

		// This catches exceptions thrown on the main thread but aren't caught by any other try-catch 
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = (Exception)e.ExceptionObject;
			HandleException(ex);
		}

		// This is a general safeguard for unobserved exceptions that can occur in async tasks
		private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
		{
			HandleException(e.Exception);
			e.SetObserved(); //Prevents the application from crashing
		}

		private void HandleException(Exception ex)
		{

			//Log the exception, 
			Console.WriteLine($"Global Exception Caught: {ex.Message}");

			// Display a global error UI alert
			if (Application.Current != null)
			{
				Application.Current.MainPage?.Dispatcher.Dispatch(async () =>
				{
					await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
				});
			}

		}

		// This method is called after a successful login
		public void NavigateToHeroPage(string username)
		{
			// Set MainPage to Hero after Login
			MainPage = new NavigationPage(new Hero(username));
		}

		// This method is called when the user logs out
		public void NavigateToLoginPage()
		{
			//Set LoginPage as the main page for logout
			MainPage = new NavigationPage(new LoginPage());
		}
	}
}