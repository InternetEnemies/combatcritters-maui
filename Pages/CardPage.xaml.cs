

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

        private void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            var viewModel = BindingContext as CardsViewModel;
            if (viewModel != null && viewModel.LoadMoreCommand.CanExecute(null))
            {
                viewModel.LoadMoreCommand.Execute(null);
            }
        }

    }

}

