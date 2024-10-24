using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.user;


namespace Combat_Critters_2._0.ViewModels
{
    public class CardsViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private ObservableCollection<ICard> _userCards;
        private List<ICard> _allUserCards;
        private const int _batchSize = 10; //Number of cards to display at a time
        private bool _hasCards; //Does a user have any card?
        private bool _showLoadMoreButton;


        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
            }
        }

        public bool ShowLoadMoreButton
        {
            get => _showLoadMoreButton;
            set
            {
                _showLoadMoreButton = value;
                OnPropertyChanged(nameof(ShowLoadMoreButton));
            }
        }
        public ObservableCollection<ICard> UserCards
        {
            get => _userCards;
            set
            {
                _userCards = value;
                OnPropertyChanged(nameof(UserCards));
            }
        }

        public ICommand LoadMoreCommand { get; }
        public CardsViewModel()
        {
            _userCards = new ObservableCollection<ICard>();
            _allUserCards = new List<ICard>();
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            LoadMoreCommand = new Command(LoadMoreCards);
            _showLoadMoreButton = false;
            HasCards = true;


            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadUserCards();
        }

        /// <summary>
        /// This loads the card  page with user cards, if any
        /// </summary>
        /// <returns></returns>
        public async Task LoadUserCards()
        {
            try
            {
                //Default query for all cards
                var userCards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

                // Check if the list is null or empty
                if (userCards != null && userCards.Count != 0)
                {
                    HasCards = true;

                    _allUserCards = userCards.Select(stack => stack.Item).ToList();


                    //Shoe the Load More button if user has cards there are more than 15 cards
                    ShowLoadMoreButton = (HasCards && (_allUserCards.Count > _batchSize));

                    //Display only the first batch of cards
                    var cardsToDisplay = _allUserCards.Take(_batchSize).ToList();
                    UserCards = new ObservableCollection<ICard>(cardsToDisplay);
                }
                else
                {
                    //User has no cards
                    HasCards = false;

                    //Clear the UserCards collection 
                    UserCards.Clear();
                }
            }
            catch (RestException)
            {
                HasCards = false;

                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user cards. Please try again.", "OK");
            }
            catch (Exception)
            {
                HasCards = false;

                throw; //bubble up to the global exception
            }

        }

        /// <summary>
        /// Call to load next batch of cards on display
        /// </summary>
        public void LoadMoreCards()
        {
            var currentCardCount = UserCards.Count;
            if (currentCardCount < _allUserCards.Count)
            {
                var nextBatch = new List<ICard>();
                nextBatch = _allUserCards.Skip(currentCardCount).Take(_batchSize).ToList();

                foreach (var card in nextBatch)
                {
                    UserCards.Add(card);
                }

                ShowLoadMoreButton = HasCards & (UserCards.Count < _allUserCards.Count);

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}