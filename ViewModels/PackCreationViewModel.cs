using System.ComponentModel;

namespace Combat_Critters_2._0.ViewModels
{
    public class PackCreationViewModel : INotifyPropertyChanged
    {
        private string _packType;
        public string PackType
        {
            get => _packType;
            set
            {
                _packType = value;
                OnPropertyChanged(nameof(PackType));
                UpdateDisplay(); //Update UI based on pack type
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private int _cardLimit;
        public int CardLimit
        {
            get => _cardLimit;
            set
            {
                _cardLimit = value;
                OnPropertyChanged(nameof(CardLimit));
            }
        }


        public PackCreationViewModel(string packType)
        {
            PackType = packType;
            //LoadAvailableCards();
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            switch (PackType)
            {
                case "Basic":
                    Description = "A basic pack with limited selection of cards.";
                    CardLimit = 5;
                    break;

                case "Advanced":
                    Description = "An advanced pack";
                    CardLimit = 4;
                    break;

                case "Premium":
                    Description = "A premium pack";
                    CardLimit = 5;
                    break;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}