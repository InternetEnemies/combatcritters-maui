using System.ComponentModel;
using CombatCrittersSharp.objects.MarketPlace.Implementations;

namespace Combat_Critters_2._0.ViewModels
{
    public class VendorDescriptionViewModel : INotifyPropertyChanged
    {
        private Vendor _vendor;
        private Offer _offer;

        public VendorDescriptionViewModel(Vendor vendor, Offer offer)
        {
            _vendor = vendor;
            _offer = offer;
        }

        public Vendor Vendor
        {
            get => _vendor;
            set
            {
                _vendor = value;
                OnPropertyChanged(nameof(Vendor));
            }
        }

        public Offer Offer
        {
            get => _offer;
            set
            {
                _offer = value;
                OnPropertyChanged(nameof(Offer));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}