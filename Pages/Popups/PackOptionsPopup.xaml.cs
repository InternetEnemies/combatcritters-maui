using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.Pages.Popups
{
    public partial class PackOptionsPopup : Popup
    {
        public PackOptionsPopup()
        {
            InitializeComponent();
        }

        private async void OnBasicClicked(object sender, EventArgs e)
        {
            await NavigateToPackCreation("Basic");
        }

        private async void OnAdvancedClicked(object sender, EventArgs e)
        {
            await NavigateToPackCreation("Advanced");
        }

        private async void OnPremiumClicked(object sender, EventArgs e)
        {
            await NavigateToPackCreation("Premium");
        }

        private async Task NavigateToPackCreation(string packType)
        {
            Close();
            if (Application.Current?.MainPage != null)
                // Navigate to PackCreationPage with the selected pack type
                await Application.Current.MainPage.Navigation.PushAsync(new PackCreationPage(packType));
        }
    }
}