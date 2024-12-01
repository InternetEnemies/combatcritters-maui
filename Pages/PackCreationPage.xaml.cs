using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace Combat_Critters_2._0.Pages
{
    public partial class PackCreationPage : ContentPage
    {
        PackCreationViewModel _viewModel;
        public PackCreationPage()
        {
            InitializeComponent();
            _viewModel = new PackCreationViewModel();
            BindingContext = _viewModel;
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


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is ICard selectedCard)
            {
                Console.WriteLine("Selection is a card");
                _viewModel.SelectedCards.Add(selectedCard);
                //Clear the selection
                ((CollectionView)sender).SelectedItem = null;
            }

        }

    }

}