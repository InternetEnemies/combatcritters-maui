using System.Collections.ObjectModel;
using System.ComponentModel;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CommunityToolkit.Maui.Core.Extensions;

namespace Combat_Critters_2._0.ViewModels
{
    public class VendorDescriptionViewModel : INotifyPropertyChanged
    {
        private Vendor _vendor;
        private List<Offer> _offer;

        private ObservableCollection<string> _vendorLevels;

        public ObservableCollection<string> VendorLevels
        {
            get => _vendorLevels;
            set
            {
                _vendorLevels = value;
                OnPropertyChanged(nameof(VendorLevels));
            }
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

        public List<Offer> Offer
        {
            get => _offer;
            set
            {
                _offer = value;
                OnPropertyChanged(nameof(Offer));
            }
        }

        public VendorDescriptionViewModel(Vendor vendor, List<Offer> offer)
        {
            _vendor = vendor;
            _offer = offer;
            _vendorLevels = new ObservableCollection<string>();

            PopulateVendorLevels();
        }


        /// <summary>
        /// This method populates the vendor level list
        /// </summary>
        public void PopulateVendorLevels()
        {
            int currentLevel = Vendor.Reputation.Level;

            for (int i = 0; i <= currentLevel; i++)
            {
                VendorLevels.Add($"LV {i}");
            }
        }




        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}