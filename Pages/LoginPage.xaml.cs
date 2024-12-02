using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
        }
    }
}


