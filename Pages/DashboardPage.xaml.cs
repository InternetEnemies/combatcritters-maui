using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.ViewModels;
using CommunityToolkit.Maui.Core;

namespace Combat_Critters_2._0.Pages
{

    public partial class DashboardPage : ContentView
    {
        public DashboardPage(string username)
        {
            InitializeComponent();

            BindingContext = new DashboardViewModel(username);
        }

        private async void OnManageUsersClicked(object sender, EventArgs e)
        {
            //Navigate to the UserBoardPage
            await Navigation.PushAsync(new UserBoardPage());
        }
    }
}