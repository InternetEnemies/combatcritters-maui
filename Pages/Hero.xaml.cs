using CombatCrittersSharp.objects.card;

namespace Combat_Critters_2._0.Pages
{
    public partial class Hero : ContentPage
    {
        private CardsPage? _cardsPage;
        private readonly string _username;

        public Hero(string username)
        {
            InitializeComponent();
            _username = username;
            //Automatically navigate to the Dashboard on page load
            NavigateToDashboard();
        }

        private void NavigateToDashboard()
        {
            //Trigger the OnDashboardClicked method
            OnDashboardClicked(this, EventArgs.Empty);
        }
        private void OnDashboardClicked(object sender, EventArgs e)
        {
            ContentArea.Content = new DashboardPage(_username);
        }

        private void OnCardsClicked(object sender, EventArgs e)
        {

            //Initialize CardsPage only once
            if (_cardsPage == null)
            {
                _cardsPage = new CardsPage();
            }

            ContentArea.Content = _cardsPage;
        }

        private void OnPacksClicked(object sender, EventArgs e)
        {
            ContentArea.Content = new PacksPage();
        }

        private void OnMarketplaceClicked(object sender, EventArgs e)
        {
            ContentArea.Content = new MarketplacePage();
        }
    }
}