using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{
    public partial class DeckPage : ContentPage
    {
        public DeckPage()
        {
            InitializeComponent();
            BindingContext = new DeckViewModel();
        }
    }
}
