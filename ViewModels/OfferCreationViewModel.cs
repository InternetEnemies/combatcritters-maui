using System.ComponentModel;

namespace Combat_Critters_2._0.ViewModels
{
    public class OfferCreationViewModel : INotifyPropertyChanged
    {

        public OfferCreationViewModel()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}