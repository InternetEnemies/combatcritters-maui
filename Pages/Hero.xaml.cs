namespace Combat_Critters_2._0.Pages
{
    public partial class Hero : ContentPage
    {
        public Hero()
        {
            InitializeComponent();

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
            ContentArea.Content = new DashboardPage();
        }

        private void OnCardsClicked(object sender, EventArgs e)
        {
            ContentArea.Content = new CardsPage();
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