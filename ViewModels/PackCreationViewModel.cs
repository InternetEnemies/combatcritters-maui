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

        // Counters for each card type in the pack
        private int _commonCount;
        private int _uncommonCount;
        private int _rareCount;
        private int _epicOrLegendaryCount;

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
                UpdateDisplay(); //Update UI based on pack type
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

        private int _cardLimit;
        public int CardLimit
        {
            get => _cardLimit;
            set
            {
                _cardLimit = value;
                OnPropertyChanged(nameof(CardLimit));
            }
        }


        public PackCreationViewModel(string packType)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _packType = packType;
            _description = "";
            _gameCards = new ObservableCollection<ICard>();
            _selectedCards = new ObservableCollection<ICard>();
            HasCards = false;

            SelectionChangedCommand = new Command<ICard>(OnCardSelected);

            // Initialize counters
            _commonCount = 0;
            _uncommonCount = 0;
            _rareCount = 0;
            _epicOrLegendaryCount = 0;

            //LoadAvailableCards();
            UpdateDisplay();
            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());

        }

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
            // Determine if the card can be added based on pack type and counters
            switch (PackType)
            {
                case "Basic":
                    if (_commonCount < 2 && selectedCard.Rarity == Rarity.COMMON)
                    {
                        SelectedCards.Add(selectedCard);
                        _commonCount++;
                    }
                    else if (_commonCount >= 2 && _uncommonCount < 1 &&
                             (selectedCard.Rarity == Rarity.UNCOMMON || selectedCard.Rarity == Rarity.RARE))
                    {
                        SelectedCards.Add(selectedCard);
                        _uncommonCount++;
                    }
                    else
                    {
                        DisplayLimitReachedMessage("Basic pack requires exactly 2 Common cards and 1 Uncommon or Rare card.");
                    }
                    break;

                case "Advanced":
                    if (_commonCount < 2 && selectedCard.Rarity == Rarity.COMMON)
                    {
                        SelectedCards.Add(selectedCard);
                        _commonCount++;
                    }
                    else if (_uncommonCount < 1 && selectedCard.Rarity == Rarity.UNCOMMON)
                    {
                        SelectedCards.Add(selectedCard);
                        _uncommonCount++;
                    }
                    else if (_rareCount < 1 && selectedCard.Rarity == Rarity.RARE)
                    {
                        SelectedCards.Add(selectedCard);
                        _rareCount++;
                    }
                    else
                    {
                        DisplayLimitReachedMessage("Advanced pack requires 2 Common cards, 1 Uncommon card, and 1 Rare card.");
                    }
                    break;

                case "Premium":
                    if (_commonCount < 2 && selectedCard.Rarity == Rarity.COMMON)
                    {
                        SelectedCards.Add(selectedCard);
                        _commonCount++;
                    }
                    else if (_uncommonCount < 1 && selectedCard.Rarity == Rarity.UNCOMMON)
                    {
                        SelectedCards.Add(selectedCard);
                        _uncommonCount++;
                    }
                    else if (_rareCount < 1 && selectedCard.Rarity == Rarity.RARE)
                    {
                        SelectedCards.Add(selectedCard);
                        _rareCount++;
                    }
                    else if (_epicOrLegendaryCount < 1 &&
                             (selectedCard.Rarity == Rarity.EPIC || selectedCard.Rarity == Rarity.LEGENDARY))
                    {
                        SelectedCards.Add(selectedCard);
                        _epicOrLegendaryCount++;
                    }
                    else
                    {
                        DisplayLimitReachedMessage("Premium pack requires 2 Common cards, 1 Uncommon card, 1 Rare card, and 1 Epic or Legendary card.");
                    }
                    break;
            }
        }

        private void DisplayLimitReachedMessage(string message)
        {
            if (Application.Current?.MainPage != null)
                Application.Current.MainPage.DisplayAlert("Limit Reached", message, "OK");
        }
        private async Task InitializeViewModelAsync()
        {
            await LoadGameCards();
        }

        /// <summary>
        /// This method uses th backend services to load all game cards
        /// </summary>
        /// <returns>Task</returns>
        public async Task LoadGameCards()
        {
            IsLoading = true;
            bool hasCards = false; // function scoped variable
            try
            {
                CardQueryBuilder filteredBuild = new CardQueryBuilder();
                //filteredBuild.SetOwned(true);
                var cards = await _backendService.GetCardsAsync(filteredBuild.Build());
                Console.WriteLine($"Received {cards?.Count} cards from backend");
                if (cards != null && cards.Count > 0)
                {

                    // Application.Current?.Dispatcher.Dispatch(() =>
                    // 
                    GameCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                    hasCards = true;
                    // });

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
        /// Update the display base on the pack type
        /// </summary>
        private void UpdateDisplay()
        {
            switch (PackType)
            {
                case "Basic":
                    Description = "A basic pack with limited selection of cards.";
                    CardLimit = 5;
                    break;

                case "Advanced":
                    Description = "An advanced pack";
                    CardLimit = 4;
                    break;

                case "Premium":
                    Description = "A premium pack";
                    CardLimit = 5;
                    break;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}