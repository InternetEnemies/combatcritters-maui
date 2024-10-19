

using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{
    public partial class CardPage : ContentPage
    {
        public CardPage()
        {
            InitializeComponent();
            BindingContext = new CardsViewModel();
        }

    }

}

