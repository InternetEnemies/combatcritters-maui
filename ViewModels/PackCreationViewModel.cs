
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
        public ICommand SelectionChangedCommand { get; }
        public ICommand CreatePackCommand { get; }

        // Limits for each card type in the pack
        private int commonLimit;
        private int uncommonLimit;
        private int rareLimit;
        private int epicLimit;
        private int legendaryLimit;

        private bool _hasCards;
        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
            }
        }

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

        private string _packType;
        public string PackType
        {
            get => _packType;
            set
            {
                _packType = value;
                OnPropertyChanged(nameof(PackType));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private readonly int CardLimit = 10;
        private Dictionary<int, int> _rarityProbabilities;



        public PackCreationViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _packType = "packType";
            _description = "";
            _gameCards = new ObservableCollection<ICard>();
            _selectedCards = new ObservableCollection<ICard>();
            HasCards = false;

            SelectionChangedCommand = new Command<ICard>(OnCardSelected);
            CreatePackCommand = new Command(async () => await CreatePackAsync());



            // Initialize raritylimits and rarity probabilities based on initial PackType
            SetRarityLimits(PackType);
            _rarityProbabilities = new Dictionary<int, int>
            {
                { (int)Rarity.COMMON, 0 },
                { (int)Rarity.UNCOMMON, 0 },
                { (int)Rarity.RARE, 0 },
                { (int)Rarity.EPIC, 0 },
                { (int)Rarity.LEGENDARY, 0 }
            };
            SetRarityProbabilities(PackType);

            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());

        }

        private async Task InitializeViewModelAsync()
        {
            await LoadGameCards();
        }

        /// <summary>
        /// Load all available game cards using backend services
        /// </summary>
        /// <returns>Task</returns>
        public async Task LoadGameCards()
        {
            IsLoading = true;
            bool hasCards = false; // function scoped variable

            try
            {
                CardQueryBuilder filteredBuild = new();
                var cards = await _backendService.GetCardsAsync(filteredBuild.Build());

                if (cards != null && cards.Count > 0)
                {
                    GameCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                    hasCards = true;
                    Console.WriteLine($"Number of cards loaded: {GameCards.Count}");
                }
                else
                {
                    //Game has no Cards
                    GameCards.Clear();
                }
            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load game cards. Please try again.", "OK");
            }

            finally
            {
                //Set HasCards based on result of operation
                HasCards = hasCards;
                IsLoading = false;
            }
        }

        /// <summary>
        /// Set rarity limits based on pack type
        /// </summary>
        /// <param name="packType">Pack Type</param>
        /// <exception cref="InvalidOperationException"></exception>
        private void SetRarityLimits(string packType)
        {
            switch (packType)
            {
                case "Basic":
                    commonLimit = 8;
                    uncommonLimit = 1;
                    rareLimit = 1;
                    epicLimit = 0;
                    legendaryLimit = 0;
                    break;

                case "Advanced":
                    commonLimit = 5;
                    uncommonLimit = 3;
                    rareLimit = 1;
                    epicLimit = 1;
                    legendaryLimit = 0;
                    break;

                case "Premium":
                    commonLimit = 3;
                    uncommonLimit = 2;
                    rareLimit = 2;
                    epicLimit = 1;
                    legendaryLimit = 1;
                    break;

                default:
                    throw new InvalidOperationException("Unknown pack type.");
            }
        }

        /// <summary>
        /// Set rarityProbabilities based on card type
        /// </summary>
        /// <param name="packType"> pack typw </param>
        private void SetRarityProbabilities(string packType)
        {
            switch (packType)
            {
                case "Basic":
                    _rarityProbabilities[(int)Rarity.COMMON] = 80;
                    _rarityProbabilities[(int)Rarity.UNCOMMON] = 15;
                    _rarityProbabilities[(int)Rarity.RARE] = 5;
                    break;

                case "Advanced":
                    _rarityProbabilities[(int)Rarity.COMMON] = 50;
                    _rarityProbabilities[(int)Rarity.UNCOMMON] = 30;
                    _rarityProbabilities[(int)Rarity.RARE] = 15;
                    _rarityProbabilities[(int)Rarity.EPIC] = 5;
                    break;

                case "Premium":
                    _rarityProbabilities[(int)Rarity.COMMON] = 30;
                    _rarityProbabilities[(int)Rarity.UNCOMMON] = 25;
                    _rarityProbabilities[(int)Rarity.RARE] = 20;
                    _rarityProbabilities[(int)Rarity.EPIC] = 15;
                    _rarityProbabilities[(int)Rarity.LEGENDARY] = 10;
                    break;
            }
        }

        /// <summary>
        /// Create Pack
        /// </summary>
        /// <returns></returns>
        public async Task CreatePackAsync()
        {
            //CardIds
            List<int> cardIds = SelectedCards.Select(card => card.CardId).ToList();

            //PackName and image
            string packName = PackType;
            string packImage = "pack.png";

            try
            {
                // IPack createdPack = await _backendService.CreatePackAsync(cardIds, _rarityProbabilities, packName, packImage);
                Console.WriteLine("Pack created successfully!");

                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Pack created successfully!", "OK");
                }
            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to create pack. Please try again.", "OK");
            }
        }

        /// <summary>
        /// Handles logic for card selection
        /// </summary>
        /// <param name="selectedCard"></param>
        public void OnCardSelected(ICard selectedCard)
        {
            if (selectedCard == null || SelectedCards.Contains(selectedCard))
                return;

            // Enforce total card limit first
            if (SelectedCards.Count >= CardLimit)
            {
                DisplayLimitReachedMessage("Maximum number of cards reached for this pack.");
                return;
            }

            // Check if the selected card's rarity limit has been reached
            switch (selectedCard.Rarity)
            {
                case Rarity.COMMON:
                    if (SelectedCards.Count(c => c.Rarity == Rarity.COMMON) < commonLimit)
                        SelectedCards.Add(selectedCard);
                    else
                        DisplayLimitReachedMessage($"Cannot add more than {commonLimit} Common cards.");
                    break;

                case Rarity.UNCOMMON:
                    if (SelectedCards.Count(c => c.Rarity == Rarity.UNCOMMON) < uncommonLimit)
                        SelectedCards.Add(selectedCard);
                    else
                        DisplayLimitReachedMessage($"Cannot add more than {uncommonLimit} Uncommon cards.");
                    break;

                case Rarity.RARE:
                    if (SelectedCards.Count(c => c.Rarity == Rarity.RARE) < rareLimit)
                        SelectedCards.Add(selectedCard);
                    else
                        DisplayLimitReachedMessage($"Cannot add more than {rareLimit} Rare cards.");
                    break;

                case Rarity.EPIC:
                    if (SelectedCards.Count(c => c.Rarity == Rarity.EPIC) < epicLimit)
                        SelectedCards.Add(selectedCard);
                    else
                        DisplayLimitReachedMessage($"Cannot add more than {epicLimit} Epic cards.");
                    break;

                case Rarity.LEGENDARY:
                    if (SelectedCards.Count(c => c.Rarity == Rarity.LEGENDARY) < legendaryLimit)
                        SelectedCards.Add(selectedCard);
                    else
                        DisplayLimitReachedMessage($"Cannot add more than {legendaryLimit} Legendary cards.");
                    break;

                default:
                    DisplayLimitReachedMessage("Invalid card rarity.");
                    break;
            }
        }

        private static void DisplayLimitReachedMessage(string message)
        {
            if (Application.Current?.MainPage != null)
                Application.Current.MainPage.DisplayAlert("Limit Reached", message, "OK");
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}