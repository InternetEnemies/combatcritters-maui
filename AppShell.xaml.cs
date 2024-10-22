using System.Windows.Input;
namespace Combat_Critters_2._0;

public partial class AppShell : Shell
{
	public ICommand GitHubRepoCommand { get; }

	public AppShell()
	{
		InitializeComponent();

		//Initialize the Command
		GitHubRepoCommand = new Command<string>(async (url) => await OpenGitHubRepo(url));
		this.BindingContext = this;
	}

	//Method to delete a user Account
	private void OnDeleteAccountClicked(object sender, EventArgs e)
	{
		//Do something, send user data to the back end

		//Redirect User to Login
		(Application.Current as App)?.NavigateToLoginPage();
	}

	//Method to log out a user
	private void OnLogoutClicked(object sender, EventArgs e)
	{
		//Maybe clear out any information 


		//This will just redirect a uses back to the login page
		(Application.Current as App)?.NavigateToLoginPage();
	}

	//Method to open the GitHub repository link
	private async Task OpenGitHubRepo(string repoUrl)
	{
		try
		{
			//Use MAUI Esssential to open the URL in the browser
			await Browser.OpenAsync(repoUrl, BrowserLaunchMode.SystemPreferred);

		}
		catch (Exception ex)
		{
			//Handle any exception that occur during URL opening
			Console.WriteLine($"Failed to open URL: {ex.Message}");
		}
	}
}
