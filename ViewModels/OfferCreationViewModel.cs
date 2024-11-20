using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CombatCrittersSharp.objects.MarketPlace.Interfaces;
using CombatCrittersSharp.objects.pack;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.ViewModels
{
    public class OfferCreationViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private Vendor _vendor; //Vendor Selected
        public Vendor Vendor
        {
            get => _vendor;
            set
            {
                _vendor = value;
                OnPropertyChanged(nameof(Vendor));
            }
        }
        public int _newLevel; //New Offer Level to be Created
        public int NewLevel
        {
            get => _newLevel;
            set
            {
                _newLevel = value;
                OnPropertyChanged(nameof(NewLevel));
            }
        }

        //UI object to hold selected Cards from inventory
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

        //Current Dev
        public Command<string> OnShowFlyoutCommand { get; }
        public Command OnCloseFlyoutCommand { get; }
        private bool _isFlyoutVisible;
        public bool IsFlyoutVisible
        {
            get => _isFlyoutVisible;
            set
            {
                _isFlyoutVisible = value;
                OnPropertyChanged(nameof(IsFlyoutVisible));
            }
        }

        //UI to hold FlyoutItems
        private ObservableCollection<object> _flyoutItems;

        public ObservableCollection<object> FlyoutItems
        {
            get => _flyoutItems;
            set
            {
                _flyoutItems = value;
                OnPropertyChanged(nameof(FlyoutItems));
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

        private ObservableCollection<IPack> _gamePacks;
        public ObservableCollection<IPack> GamePacks
        {
            get => _gamePacks;
            set
            {
                _gamePacks = value;
                OnPropertyChanged(nameof(GamePacks));

            }
        }


        public OfferCreationViewModel(Vendor vendor)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _vendor = vendor;
            NewLevel = Vendor.Reputation.Level + 1;
            _selectedCards = new ObservableCollection<ICard>();
            _flyoutItems = new ObservableCollection<object>();

            //Inventory Initialization
            _gameCards = new ObservableCollection<ICard>();
            _gamePacks = new ObservableCollection<IPack>();


            //Load Cards and Packs
            Task.Run(async () => await InitializeViewModelAsync());

            //CurrentDev
            IsFlyoutVisible = false;
            OnShowFlyoutCommand = new Command<string>(ShowFlyout);
            OnCloseFlyoutCommand = new Command(() => IsFlyoutVisible = false);
        }
        private async Task InitializeViewModelAsync()
        {
            await LoadGameCards();
            await LoadPacks();
        }

        /// <summary>
        /// Loads game cards using predifined filter
        /// </summary>
        public async Task LoadGameCards()
        {
            try
            {
                var query = new CardQueryBuilder().Build();

                var cards = await _backendService.GetCardsAsync(query);

                if (cards != null && cards.Count > 0)
                {
                    GameCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                    Console.WriteLine($"Number of cards loaded: {GameCards.Count}");
                }
                else
                {
                    //Game has no Cards
                    GameCards.Clear();
                    Console.WriteLine("No cards found for the user.");
                }

            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user cards. Please try again.", "OK");
            }
            catch (Exception)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
            }
        }

        private async Task LoadPacks()
        {
            try
            {
                var packs = await _backendService.GetPacksAsync(); //Get all packs in the game

                if (packs != null && packs.Count > 0)
                {
                    GamePacks = new ObservableCollection<IPack>(packs);
                }
                else
                {
                    GamePacks.Clear();
                }

            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load users. Please try again.", "OK");
            }
        }


        /// <summary>
        /// The flyoutItem will be populated based on inventoryType
        /// </summary>
        /// <param name="inventoryType">This is the OnShowFlyoutCommand pat</param>
        private void ShowFlyout(string inventoryType)
        {
            IsFlyoutVisible = true;
            switch (inventoryType)
            {

                case "CardInventory":
                    FlyoutItems.Clear();

                    //Add the cards to the flyoutItems list
                    foreach (var item in GameCards)
                    {
                        FlyoutItems.Add(item);
                    }
                    break;

                case "PackInventory":
                    FlyoutItems.Clear();

                    //Add the packs to the flyoutItems list
                    foreach (var item in GamePacks)
                    {
                        FlyoutItems.Add(item);
                    }
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