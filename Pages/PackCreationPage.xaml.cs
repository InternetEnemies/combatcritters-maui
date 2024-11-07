using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.card.Interfaces;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.Pages
{
    public partial class PackCreationPage : ContentPage
    {

        public PackCreationPage(string packType)
        {
            InitializeComponent();
            var viewModel = new PackCreationViewModel(packType);
            BindingContext = viewModel;
        }
        private void OnCardDragStarting(object sender, DragStartingEventArgs e)
        {
            if (sender is Element element && element.BindingContext is ICard draggedCard)
            {
                e.Data.Properties.Add("DraggedCard", draggedCard);
            }
        }

        private void OnCardDropCompleted(object sender, DropEventArgs e)
        {
            if (e.Data.Properties.TryGetValue("DraggedCard", out object draggedCardObj) &&
                draggedCardObj is ICard draggedCard)
            {
                var viewModel = BindingContext as PackCreationViewModel;
                if (viewModel != null && viewModel.GameCards.Count < viewModel.CardLimit)
                {
                    viewModel.GameCards.Remove(draggedCard);
                    // Add to SelectedCards or other relevant collection in your ViewModel
                }
                else
                {
                    DisplayAlert("Limit Reached", $"You can only add {viewModel.CardLimit} cards to this pack.", "OK");
                }
            }
        }

    }

}