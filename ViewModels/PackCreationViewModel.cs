
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

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

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

        //GAME CARDS
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

        //GAME PACK IMAGES
        private ObservableCollection<string> _gamePackImagesURL;
        public ObservableCollection<string> GamePackImagesURL
        {
            get => _gamePackImagesURL;
            set
            {
                _gamePackImagesURL = value;
                OnPropertyChanged(nameof(GamePackImagesURL));
            }
        }
        public PackCreationViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _packName = "";
            _gameCards = new ObservableCollection<ICard>();
            _gamePackImagesURL = new ObservableCollection<string>();

            Task.Run(async () => await LoadDataNeeded());
        }

        private async Task LoadDataNeeded()
        {
            IsLoading = true;
            try
            {
                //Update Game Cards
                GameCards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

                var root = "https://combatcritters.s3.us-east-1.amazonaws.com/";
                //Loading Game Pack Images
                for (int i = 0; i < 5; i++)
                {
                    var packURL = root + $"pack{i}.png";
                    GamePackImagesURL.Add(root + $"pack{i}.png");
                }
                foreach (var url in GamePackImagesURL)
                {
                    Console.WriteLine(url);
                }

            }
            finally
            {
                IsLoading = false;
            }


        }




        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}