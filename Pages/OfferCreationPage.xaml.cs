using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{
    public partial class OfferCreationPage : ContentPage
    {
        public OfferCreationPage()
        {
            InitializeComponent();
            BindingContext = new OfferCreationViewModel();
        }
    }
}