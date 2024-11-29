
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;

namespace Combat_Critters_2._0.ViewModels
{
    public class PackCreationViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;

        //PACK NAME
        private string _packName;
        public string PackName
        {
            get => _packName;
            set
            {
                _packName = value;
                OnPropertyChanged(nameof(PackName));
            }
        }

        private ObservableCollection<ICard> _gameCards;
        public ObservableCollection<ICard> GameCards
        {
            get => _gameCards;
            set
            {
                _gameCards = value;
                OnPropertyChanged(nameof(GameCards));
            }
        }

        public PackCreationViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _packName = "";
            _gameCards = new ObservableCollection<ICard>();

            Task.Run(async () => await LoadDataNeeded());
        }

        private async Task LoadDataNeeded()
        {

            //Update Game Cards
            GameCards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

        }




        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}