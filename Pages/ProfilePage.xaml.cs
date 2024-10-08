using Combat_Critters_2._0.ViewModels;
using Microsoft.Maui.Controls;

namespace Combat_Critters_2._0.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
        }

        // This method will share a user profile
        public void OnShareClicked(object sender, EventArgs e)
        {
            //To be implemented
        }
    }

}


