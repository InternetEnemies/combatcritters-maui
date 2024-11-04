using Combat_Critters_2._0.ViewModels;
namespace Combat_Critters_2._0.Pages
{
    public partial class UserBoardPage : ContentPage
    {
        public UserBoardPage()
        {
            InitializeComponent();
            BindingContext = new UserBoardViewModel();
        }

        /// <summary>
        /// This method handled the search text change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as UserBoardViewModel;
            viewModel?.FilterUsers(e.NewTextValue);
        }
    }
}