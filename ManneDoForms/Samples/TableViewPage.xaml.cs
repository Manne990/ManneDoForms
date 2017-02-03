using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class TableViewPage : ContentPage
    {
        public TableViewPage()
        {
            InitializeComponent();
            this.BindingContext = new TableViewPageViewModel();
        }
    }

    public class TableViewPageViewModel : INotifyPropertyChanged
    {
        public TableViewPageViewModel()
        {
            // Add some data to the model
            this.DummyItems = new ObservableCollection<ObservableCollection<string>>();

            for (int col = 0; col < 4; col++)
            {
                var items = new ObservableCollection<string>();

                for (int row = 0; row < 7; row++)
                {
                    items.Add($"{row}, {col}");
                }

                this.DummyItems.Add(items);
            }
        }

        private ObservableCollection<ObservableCollection<string>> _dummyItems;
        public ObservableCollection<ObservableCollection<string>> DummyItems
        {
            get { return _dummyItems; }
            set
            {
                if (value != _dummyItems)
                {
                    _dummyItems = value;
                    NotifyPropertyChanged("DummyItems");
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