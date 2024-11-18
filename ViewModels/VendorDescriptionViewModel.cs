using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.Maui.Controls;

namespace Combat_Critters_2._0.ViewModels
{
    public class VendorDescriptionViewModel : INotifyPropertyChanged
    {
        private Vendor _vendor;
        private List<Offer> _offer;

        private Offer? _selectedOffer; // This can be null
        private ObservableCollection<string> _vendorLevels;
        private ObservableCollection<object?> _receiveItems;
        private ObservableCollection<object?> _giveItems;


        private string? _selectedLevel;

        public ObservableCollection<object?> GiveItems
        {
            get => _giveItems;
            set
            {
                _giveItems = value;
                OnPropertyChanged(nameof(GiveItems));
            }
        }
        public ObservableCollection<object?> ReceiveItems
        {
            get => _receiveItems;
            set
            {
                _receiveItems = value;
                OnPropertyChanged(nameof(ReceiveItems));
            }
        }
        public Offer? SelectedOffer
        {
            get => _selectedOffer;
            set
            {
                _selectedOffer = value;
                OnPropertyChanged(nameof(SelectedOffer));
            }
        }

        public string? SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                _selectedLevel = value;
                OnPropertyChanged(nameof(SelectedLevel));
                OnLevelSelected(value);
            }
        }
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
            _receiveItems = new ObservableCollection<object?>();
            _giveItems = new ObservableCollection<object?>();
            PopulateVendorLevels();
        }


        /// <summary>
        /// Update the Offer based on selected Level
        /// </summary>
        /// <param name="selectedLevel"></param>
        private void OnLevelSelected(string selectedLevel)
        {
            if (int.TryParse(selectedLevel?.Replace("LV ", ""), out int level) && level >= 0 && level < Offer.Count)
            {
                //Update the selctedOffer
                SelectedOffer = Offer[level];
                UpdateItems();

            }
        }

        private void UpdateItems()
        {
            ReceiveItems.Clear();
            GiveItems.Clear();

            if (SelectedOffer?.Receive != null)
            {
                var item = SelectedOffer.Receive.ParsedItem;
                ReceiveItems.Add(item); //Add the parsed Item
            }

            if (SelectedOffer?.Give != null)
            {
                foreach (var item in SelectedOffer.Give)
                {
                    GiveItems.Add(item.ParsedItem);
                }
            }
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