
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
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

        private int? _slot5Weight;
        public int? Slot5Weight
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

                    var toast = Toast.Make("Pack Image Selected", ToastDuration.Short, 14);
                    toast.Show();

                }
            }
        }

        //SELECTED CARDS
        private ObservableCollection<ICard> _selectedCards;
        public ObservableCollection<ICard> SelectedCards
        {
            get => _selectedCards;
            set
            {
                _selectedCards = value;
                OnPropertyChanged(nameof(SelectedCards));
            }
        }

        public ICommand OnCreateCommand { get; }

        public PackCreationViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _packName = "";
            _selectedPackImage = "";
            _gameCards = new ObservableCollection<ICard>();
            _gamePackImagesURL = new ObservableCollection<string>();

            _selectedCards = new ObservableCollection<ICard>();
            OnCreateCommand = new Command(async () => await CreateCommandAsync());

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

        private async Task CreateCommandAsync()
        {
            try
            {
                validateOnCreate();

                int[] cardIds = SelectedCards.Select(card => card.CardId).ToArray();
                List<Dictionary<int, int>> slotRarityProbabilities = new List<Dictionary<int, int>>();

                if (Slot1Rarity.HasValue && Slot1Weight.HasValue)
                    AddSlotData(slotRarityProbabilities, Slot1Rarity.Value, Slot1Weight.Value);

                if (Slot2Rarity.HasValue && Slot2Weight.HasValue)
                    AddSlotData(slotRarityProbabilities, Slot2Rarity.Value, Slot2Weight.Value);

                if (Slot3Rarity.HasValue && Slot3Weight.HasValue)
                    AddSlotData(slotRarityProbabilities, Slot3Rarity.Value, Slot3Weight.Value);

                if (Slot4Rarity.HasValue && Slot4Weight.HasValue)
                    AddSlotData(slotRarityProbabilities, Slot4Rarity.Value, Slot4Weight.Value);

                if (Slot5Rarity.HasValue && Slot5Weight.HasValue)
                    AddSlotData(slotRarityProbabilities, Slot5Rarity.Value, Slot5Weight.Value);


                var pack = await _backendService.CreatePackAsync(PackName, SelectedPackImage, cardIds, slotRarityProbabilities);

                if (pack != null)
                {
                    var toast = Toast.Make($"Pack Created Successfully", ToastDuration.Short);
                    await toast.Show();
                }
                else
                {
                    var toast = Toast.Make($"Pack Creation Failed. Try Again", ToastDuration.Short);
                    await toast.Show();
                }
            }
            catch (ArgumentException e)
            {
                var toast = Toast.Make(e.Message, ToastDuration.Short);
                await toast.Show();
            }
            catch (InvalidOperationException)
            {
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }
            catch (RestException)
            {
                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException)
            {
                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }
        }

        private void validateOnCreate()
        {
            //
            if (string.IsNullOrWhiteSpace(PackName) || PackName == "")
            {
                throw new ArgumentException("Please Enter Pack Name");
            }
            else if (string.IsNullOrWhiteSpace(SelectedPackImage) || SelectedPackImage == "")
            {
                throw new ArgumentException("Please Choose a Pack Image");
            }
            else if (SelectedCards.Count == 0)
            {
                throw new ArgumentException("Please Add Cards to your pack");
            }

            // Validate Slot Entries
            ValidateSlot("Slot 1", Slot1Weight, Slot1Rarity);
            ValidateSlot("Slot 2", Slot2Weight, Slot2Rarity);
            ValidateSlot("Slot 3", Slot3Weight, Slot3Rarity);
            ValidateSlot("Slot 4", Slot4Weight, Slot4Rarity);
            ValidateSlot("Slot 5", Slot5Weight, Slot5Rarity);

        }

        /// <summary>
        /// Validate Slot Entries
        /// </summary>
        /// <param name="slotName"></param>
        /// <param name="weight"></param>
        /// <param name="rarity"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateSlot(string slotName, int? weight, int? rarity)
        {
            // Validate Weight
            if (!weight.HasValue || weight < 0 || weight > 100)
            {
                throw new ArgumentException($"{slotName} weight must be a number between 1 and 100.");
            }

            // Validate Rarity
            if (rarity.HasValue && (rarity < 1 || rarity > 5))
            {
                throw new ArgumentException($"{slotName} rarity must be a number between 1 and 5.");
            }
        }

        /// <summary>
        /// Helper method to add slot data
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="rarity"></param>
        /// <param name="weight"></param>
        private void AddSlotData(List<Dictionary<int, int>> slot, int rarity, int weight)
        {
            slot.Add(new Dictionary<int, int> { { rarity, weight } });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}