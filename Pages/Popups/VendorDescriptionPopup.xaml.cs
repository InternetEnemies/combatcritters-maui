using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.MarketPlace.Implementations;

using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.Pages.Popups
{
    public partial class VendorDescriptionPopup : Popup
    {
        public VendorDescriptionPopup(Vendor vendor, Offer offer)
        {
            InitializeComponent();
            BindingContext = new VendorDescriptionViewModel(vendor, offer);
        }
    }

}