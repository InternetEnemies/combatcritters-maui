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
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
        public string _newLevel; //New Offer Level to be Created
        public string NewLevel
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

        private bool _isCurrencyVisible;
        public bool IsCurrencyVisible
        {
            get => _isCurrencyVisible;
            set
            {
                _isCurrencyVisible = value;
                OnPropertyChanged(nameof(IsCurrencyVisible));
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

        //UI to hold object selected for I Collect
        private ObservableCollection<object> _iCollectItems;

        public ObservableCollection<object> ICollectItems
        {
            get => _iCollectItems;
            set
            {
                _iCollectItems = value;
                OnPropertyChanged(nameof(ICollectItems));
            }
        }

        //UI to hold object selected for I Give
        private ObservableCollection<object> _iGiveItems;
        public ObservableCollection<object> IGiveItems
        {
            get => _iGiveItems;
            set
            {
                _iGiveItems = value;
                OnPropertyChanged(nameof(IGiveItems));
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

        private string currentSelection = "";
        public ICommand OnCreateOfferCommand { get; }

        public OfferCreationViewModel(Vendor vendor)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _vendor = vendor;
            _selectedCards = new ObservableCollection<ICard>();
            _flyoutItems = new ObservableCollection<object>();

            //Inventory Initialization
            _gameCards = new ObservableCollection<ICard>();
            _gamePacks = new ObservableCollection<IPack>();
            _iCollectItems = new ObservableCollection<object>();
            _iGiveItems = new ObservableCollection<object>();
            _newLevel = "";


            //Load Cards and Packs
            Task.Run(async () => await LoadDataNeeded());

            //CurrentDev
            IsFlyoutVisible = false;
            IsCurrencyVisible = false;
            OnShowFlyoutCommand = new Command<string>(ShowFlyout);
            OnCloseFlyoutCommand = new Command(() => IsFlyoutVisible = false);
            OnCreateOfferCommand = new Command(CreateOffer);
        }

        private async Task LoadDataNeeded()
        {
            //IsLoading = true;
            try
            {
                GameCards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

                var packs = await _backendService.GetPacksAsync(); //Get all packs in the game
                if (packs != null)
                    GamePacks = new ObservableCollection<IPack>(packs);
            }
            catch (InvalidOperationException e)
            {
                //Log
                Console.WriteLine(e.Message);

                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }
            catch (RestException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException e)
            {
                //Log
                Console.WriteLine(e.Message);

                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }
        }

        /// <summary>
        /// The flyoutItem will be populated based on inventoryType
        /// </summary>
        /// <param name="inventoryType">This is the OnShowFlyoutCommand pat</param>
        private void ShowFlyout(string inventoryType)
        {

            string[] parts = inventoryType.Split(':'); // Split the string by ':'
            currentSelection = parts[0];

            IsFlyoutVisible = true;
            switch (parts[1])
            {
                case "CardInventory":
                    IsCurrencyVisible = false;
                    FlyoutItems.Clear();

                    //Add the cards to the flyoutItems list
                    foreach (var item in GameCards)
                    {
                        FlyoutItems.Add(item);
                    }
                    break;

                case "PackInventory":
                    IsCurrencyVisible = false;
                    FlyoutItems.Clear();

                    //Add the packs to the flyoutItems list
                    foreach (var item in GamePacks)
                    {
                        FlyoutItems.Add(item);
                    }
                    break;

                case "CurrencyInventory":
                    FlyoutItems.Clear();

                    //Display the Currency template
                    IsCurrencyVisible = true;
                    break;
            }

        }

        public void OnFlyoutItmeSelected(object e)
        {
            if (currentSelection == "ICollect")
                //Add the selectedItem to ICollectItems List
                ICollectItems.Add(e);
            else if (currentSelection == "IGive")
                IGiveItems.Add(e);
        }

        public void OnICollectItemSelected(object e)
        {
            //Remove the ICollectItem 
            ICollectItems.Remove(e);
        }
        public void OnIGiveItemSelected(object e)
        {
            IGiveItems.Remove(e);
        }
        /// <summary>
        /// On Create, the Object are converted into OfferCreationItems
        /// </summary>
        public async void CreateOffer()
        {
            try
            {
                var collectList = CreateItems(ICollectItems);
                var giveList = CreateItems(IGiveItems);

                //Send information to the backendservice
                var offer = _backendService.CreateNewVendorOfferAsync(Vendor.Id, Int32.Parse(NewLevel), collectList, giveList[0]);

                //Console.WriteLine($"Offer is {offer}");

            }
            catch (ArgumentException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Note: A valid vendor must give only 1 Item", "OK");
            }
            catch (AuthException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid Client", "OK");
            }
        }

        private List<OfferCreationItem> CreateItems(ObservableCollection<object> items)
        {
            //Group items by their type and ID
            var itemCounts = new Dictionary<(int? itemId, string type), int>();

            foreach (var item in items)
            {
                if (item is CombatCrittersSharp.objects.card.CardCritter cardCritter)
                {
                    var key = (itemId: (int?)cardCritter.CardId, type: "card");
                    if (itemCounts.ContainsKey(key))
                        itemCounts[key]++;
                    else
                        itemCounts[key] = 1;
                }
                else if (item is CombatCrittersSharp.objects.card.CardItem cardItem)
                {
                    var key = (itemId: (int?)cardItem.CardId, type: "card");
                    if (itemCounts.ContainsKey(key))
                        itemCounts[key]++;
                    else
                        itemCounts[key] = 1;
                }
                else if (item is CombatCrittersSharp.objects.pack.Pack pack)
                {
                    var key = (itemId: (int?)pack.PackId, type: "pack");
                    if (itemCounts.ContainsKey(key))
                        itemCounts[key]++;
                    else
                        itemCounts[key] = 1;
                }
                else if (item is CombatCrittersSharp.objects.currency.Currency currency)
                {
                    var key = (itemId: (int?)null, type: "currency");
                    if (itemCounts.ContainsKey(key))
                        itemCounts[key] += currency._coins;
                    else
                        itemCounts[key] = currency._coins;
                }
                else
                {
                    throw new InvalidOperationException($"Unknown item type: {item.GetType().Name}");
                }
            }

            //Convert the dictionary into a list of OfferCreationItems
            var offerCreationItems = new List<OfferCreationItem>();
            foreach (var kvp in itemCounts)
            {
                offerCreationItems.Add(new OfferCreationItem(
                    count: kvp.Value,
                    itemId: kvp.Key.itemId,
                    type: kvp.Key.type
                ));
            }
            return offerCreationItems;
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}