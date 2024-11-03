using Combat_Critters_2._0.ViewModels;

namespace Combat_Critters_2._0.Pages
{
    public partial class PacksPage : ContentView
    {
        public PacksPage()
        {
            InitializeComponent();
            BindingContext = new PacksViewModel();
        }

        /// <summary>
        /// This method handled the search text change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as PacksViewModel;
            viewModel?.FilterPacks(e.NewTextValue);
        }
    }
}