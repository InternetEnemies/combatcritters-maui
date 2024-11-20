using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.MarketPlace.Implementations;

namespace Combat_Critters_2._0.Pages
{
    public partial class OfferCreationPage : ContentPage
    {
        public Vendor Vendor { get; }
        public OfferCreationPage(Vendor vendor)
        {
            Vendor = vendor;
            InitializeComponent();
            BindingContext = new OfferCreationViewModel(vendor);
        }
    }
}