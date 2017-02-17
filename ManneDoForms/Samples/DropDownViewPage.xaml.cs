using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class DropDownViewPage : ContentPage
    {
        public DropDownViewPage()
        {
            InitializeComponent();

            BindingContext = new DropDownViewPageViewModel();
        }
    }

    public class DropDownViewPageViewModel : INotifyPropertyChanged
    {
        public DropDownViewPageViewModel()
        {
            // Add some data to the model
            DummyItems = new ObservableCollection<string> { "Item 1", "Item 2", "Item 3", "Item 4" };
            SelectedItem = DummyItems[1];
        }

        private ObservableCollection<string> _dummyItems;
        public ObservableCollection<string> DummyItems
        {
            get { return _dummyItems; }
            set
            {
                if (value != _dummyItems)
                {
                    _dummyItems = value;
                    NotifyPropertyChanged(nameof(DummyItems));
                }
            }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                    NotifyPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}