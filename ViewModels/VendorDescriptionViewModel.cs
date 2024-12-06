using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CombatCrittersSharp.objects.MarketPlace.Interfaces;
using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.Maui.Controls;

namespace Combat_Critters_2._0.ViewModels
{
    public class VendorDescriptionViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private Vendor _vendor;
        public Vendor Vendor
        {
            get => _vendor;
            set
            {
                _vendor = value;
                OnPropertyChanged(nameof(Vendor));
            }
        }
        private List<Offer> _offer;

        public List<Offer> Offer
        {
            get => _offer;
            set
            {
                _offer = value;
                OnPropertyChanged(nameof(Offer));
            }
        }

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

        private string? _selectedLevel;
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
        private Offer? _selectedOffer; // This can be null
        public Offer? SelectedOffer
        {
            get => _selectedOffer;
            set
            {
                _selectedOffer = value;
                OnPropertyChanged(nameof(SelectedOffer));
            }
        }

        private object _receiveItem;
        private ObservableCollection<object> _giveItems;

        public ObservableCollection<object> GiveItems
        {
            get => _giveItems;
            set
            {
                _giveItems = value;
                OnPropertyChanged(nameof(GiveItems));
            }
        }
        public object ReceiveItem
        {
            get => _receiveItem;
            set
            {
                _receiveItem = value;
                OnPropertyChanged(nameof(ReceiveItem));
                OnPropertyChanged(nameof(ReceiveItemAsCollection)); // Notify that the wrapped collection has changed
            }
        }

        public IEnumerable<object> ReceiveItemAsCollection =>
            _receiveItem != null ? new[] { _receiveItem } : Enumerable.Empty<object>();


        public VendorDescriptionViewModel(Vendor vendor, List<Offer> offer)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _vendor = vendor;
            _offer = offer;
            _vendorLevels = new ObservableCollection<string>();
            PopulateVendorLevels();

            //_receiveItems = IOfferItem>();
            _giveItems = new ObservableCollection<object>();

        }


        /// <summary>
        /// Update the Offer based on selected Level
        /// </summary>
        /// <param name="selectedLevel"></param>
        private void OnLevelSelected(string? selectedLevel)
        {
            if (int.TryParse(selectedLevel?.Replace("LV ", ""), out int level) && level >= 0)
            {
                //Find the Offer with Offer.Id = level
                var matchingOffer = Offer.FirstOrDefault(o => o.Id == level);

                if (matchingOffer != null)
                {
                    //Clear current offer items
                    GiveItems.Clear();

                    //Update the selectedOffer
                    SelectedOffer = matchingOffer;
                    foreach (var item in SelectedOffer.Give)
                    {
                        if (item.Item != null)
                            GiveItems.Add(item.Item);
                    }
                    ReceiveItem = SelectedOffer.Receive.Item;
                }
            }
        }
        /// <summary>
        /// This method populated the vendor offers
        /// </summary>
        public async void PopulateVendorLevels()
        {
            List<Offer> offers = await _backendService.GetVendorOfferAsync(Vendor.Id);
            Offer = offers;
            for (int i = 0; i < Offer.Count; i++)
            {
                var item = Offer[i];

                //Concat the string LV with the offer id.
                VendorLevels.Add($"LV {item.Id}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}