using System.Collections.ObjectModel;
using Combat_Critters_2._0.ViewModels;
using CombatCrittersSharp.objects.card.Interfaces;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.Pages.Popups
{
    public partial class PackPopup : Popup
    {
        public PackPopup(ObservableCollection<ICard> selectedCards)
        {
            InitializeComponent();
            BindingContext = new PackPopupViewModel(selectedCards);
        }
    }
}