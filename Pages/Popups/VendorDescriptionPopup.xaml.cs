using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.MarketPlace.Implementations;

using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace Combat_Critters_2._0.Pages.Popups
{
    public partial class VendorDescriptionPopup : Popup
    {
        public Vendor Vendor { get; }
        public VendorDescriptionPopup(Vendor vendor, List<Offer> offer)
        {
            Vendor = vendor;
            InitializeComponent();
            BindingContext = new VendorDescriptionViewModel(vendor, offer);
        }

        private async void OnNewLevelClicked(object sender, EventArgs e)
        {
            await NavigateToOfferCreation();
        }
        private async Task NavigateToOfferCreation()
        {
            Close();
            if (Application.Current?.MainPage != null)
                // Navigate to PackCreationPage with the selected pack type
                await Application.Current.MainPage.Navigation.PushAsync(new OfferCreationPage(Vendor));
        }


    }

}