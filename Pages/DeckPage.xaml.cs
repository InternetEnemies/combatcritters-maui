using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{
    public partial class DeckPage : ContentPage
    {
        private DeckViewModel _viewModel;
        public DeckPage()
        {
            InitializeComponent();
            _viewModel = new DeckViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await _viewModel.LoadUserDecks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading cards: {ex.Message}");

                await DisplayAlert("Error", "Failed to load user cards.", "OK");
            }
        }
    }
}
