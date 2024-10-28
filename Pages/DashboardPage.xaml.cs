using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{

    public partial class DashboardPage : ContentView
    {
        public DashboardPage(string username)
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel(username);
        }
    }
}