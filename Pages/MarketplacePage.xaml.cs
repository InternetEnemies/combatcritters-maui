using Combat_Critters_2._0.ViewModels;
namespace Combat_Critters_2._0.Pages
{
    public partial class MarketplacePage : ContentView
    {
        public MarketplacePage()
        {
            InitializeComponent();
            BindingContext = new MarketPlaceViewModel();
        }
    }
}