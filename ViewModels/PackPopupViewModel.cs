using System.Collections.ObjectModel;
using System.Windows.Input;
using CombatCrittersSharp.objects.card.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Combat_Critters_2._0.ViewModels
{
    public partial class PackPopupViewModel : ObservableObject
    {

        public ObservableCollection<ICard> SelectedPackCards { get; set; }

        public PackPopupViewModel(ObservableCollection<ICard> selectedCards)
        {
            SelectedPackCards = selectedCards;
        }
    }
}
