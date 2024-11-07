using Combat_Critters_2._0.ViewModels;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.Pages
{
    public partial class PackCreationPage : ContentPage
    {

        public PackCreationPage(string packType)
        {
            InitializeComponent();
            BindingContext = new PackCreationViewModel(packType); //
        }

    }

}