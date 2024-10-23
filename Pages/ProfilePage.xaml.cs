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
    }

}


