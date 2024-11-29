using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.card.Interfaces;


namespace Combat_Critters_2._0.Pages
{
    public partial class PackCreationPage : ContentPage
    {
        PackCreationViewModel _viewModel;
        public PackCreationPage()
        {
            InitializeComponent();
            _viewModel = new PackCreationViewModel();
            //BindingContext = new PackCreationViewModel();
        }

        /// <summary>
        /// Update the Pack name when Pack name entry is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPackNameEntryCompleted(object sender, EventArgs e)
        {
            string text = ((Entry)sender).Text;
        }

        // private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //     if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is ICard selectedCard)
        //     {
        //         var viewModel = BindingContext as PackCreationViewModel;
        //         viewModel?.OnCardSelected(selectedCard);  // Call OnCardSelected instead of directly adding to SelectedCards

        //         // Clear the selection
        //         ((CollectionView)sender).SelectedItem = null;
        //     }
        // }

    }

}