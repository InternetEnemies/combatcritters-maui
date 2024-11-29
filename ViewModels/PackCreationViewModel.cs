
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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

        //RARITY AND WEIGHTS **This is repetitive. But i'll leave it as it :)**
        private int? _slot1Rarity;
        public int? Slot1Rarity
        {
            get => _slot1Rarity;
            set
            {
                _slot1Rarity = value;
                OnPropertyChanged(nameof(Slot1Rarity));
            }
        }

        private int? _slot1Weight;
        public int? Slot1Weight
        {
            get => _slot1Weight;
            set
            {
                _slot1Weight = value;
                OnPropertyChanged(nameof(Slot1Weight));
            }
        }

        //SLOT 2
        private int? _slot2Rarity;
        public int? Slot2Rarity
        {
            get => _slot2Rarity;
            set
            {
                _slot2Rarity = value;
                OnPropertyChanged(nameof(Slot2Rarity));
            }
        }

        private int? _slot2Weight;
        public int? Slot2Weight
        {
            get => _slot2Weight;
            set
            {
                _slot2Weight = value;
                OnPropertyChanged(nameof(Slot2Weight));
            }
        }

        //SLOT 3
        private int? _slot3Rarity;
        public int? Slot3Rarity
        {
            get => _slot3Rarity;
            set
            {
                _slot3Rarity = value;
                OnPropertyChanged(nameof(Slot3Rarity));
            }
        }

        private int? _slot3Weight;
        public int? Slot3Weight
        {
            get => _slot3Weight;
            set
            {
                _slot3Weight = value;
                OnPropertyChanged(nameof(Slot3Weight));
            }
        }

        //SLOT 4
        private int? _slot4Rarity;
        public int? Slot4Rarity
        {
            get => _slot4Rarity;
            set
            {
                _slot4Rarity = value;
                OnPropertyChanged(nameof(Slot4Rarity));
            }
        }

        private int? _slot4Weight;
        public int? Slot4Weight
        {
            get => _slot4Weight;
            set
            {
                _slot4Weight = value;
                OnPropertyChanged(nameof(Slot4Weight));
            }
        }

        //SLOT 5
        private int? _slot5Rarity;
        public int? Slot5Rarity
        {
            get => _slot5Rarity;
            set
            {
                _slot5Rarity = value;
                OnPropertyChanged(nameof(Slot5Rarity));
            }
        }

        private int _slot5Weight;
        public int Slot5Weight
        {
            get => _slot5Weight;
            set
            {
                _slot5Weight = value;
                OnPropertyChanged(nameof(Slot5Weight));
            }
        }

        //SELECTED PACK IMAGE
        private string? _selectedPackImage;
        public string? SelectedPackImage
        {
            get => _selectedPackImage;
            set
            {
                if (_selectedPackImage != value)
                {
                    _selectedPackImage = value;
                    OnPropertyChanged(nameof(SelectedPackImage));
                    string text = "Pack Image Selected";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;
                    var cancellationTokenSource = new CancellationTokenSource();

                    var toast = Toast.Make(text, duration, fontSize);
                    toast.Show(cancellationTokenSource.Token);
                }
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

        private void OnSelectedPackImage()
        {

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