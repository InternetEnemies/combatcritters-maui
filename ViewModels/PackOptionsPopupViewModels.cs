using System.ComponentModel;

namespace Combat_Critters_2._0.ViewModels
{
    public class PackOptionsPopupViewModel : INotifyPropertyChanged
    {

        public PackOptionsPopupViewModel()
        {

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}