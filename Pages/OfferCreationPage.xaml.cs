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

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var viewModel = BindingContext as OfferCreationViewModel;
                viewModel?.OnFlyoutItmeSelected(e.CurrentSelection[0]);

                // Clear the selection
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private void OnRemoveICollectItem(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var viewModel = BindingContext as OfferCreationViewModel;
                viewModel?.OnICollectItemSelected(e.CurrentSelection[0]);

                // Clear the selection
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private void OnRemoveIGiveItem(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var viewModel = BindingContext as OfferCreationViewModel;
                viewModel?.OnIGiveItemSelected(e.CurrentSelection[0]);

                // Clear the selection
                ((CollectionView)sender).SelectedItem = null;
            }
        }

    }
}