
using Combat_Critters_2._0.ViewModels;
namespace Combat_Critters_2._0.Pages
{
    public partial class CardsPage : ContentView
    {
        public CardsPage()
        {
            InitializeComponent();
            BindingContext = new CardsViewModel();
        }

    }

}

