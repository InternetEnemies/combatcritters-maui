namespace Combat_Critters_2._0.Pages
{
    public partial class CreateAccountPage : ContentPage
    {
        public CreateAccountPage()
        {
            InitializeComponent();
            BindingContext = new CreateAccountViewModel(Navigation);
        }
    }
}