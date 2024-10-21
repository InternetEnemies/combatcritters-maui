﻿using Combat_Critters_2._0.Pages;

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

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = (Exception)e.ExceptionObject;
			HandleException(ex);
		}

		private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
		{
			HandleException(e.Exception);
			e.SetObserved(); //Prevents the application from crashing
		}

		private void HandleException(Exception ex)
		{

			//Log the exception,  show a UI ALERT
			Console.WriteLine($"Global Exception Caught: {ex.Message}");
			Application.Current.MainPage?.DisplayAlert("Error", "An unexpected error occured. Please try again later", "OK");
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