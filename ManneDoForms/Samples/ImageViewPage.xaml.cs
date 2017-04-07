using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace ManneDoForms.Samples
{
    public partial class ImageViewPage : ContentPage
    {
        public ImageViewPage()
        {
            InitializeComponent();

            BindingContext = new ImageViewPageViewModel();
        }
    }

    public class ImageViewPageViewModel : INotifyPropertyChanged
    {
        public ImageViewPageViewModel()
        {
            // Add some data to the model
            DummyItems = new ObservableCollection<string> 
            { 
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild1.jpg", 
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild2.jpg", 
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild3.jpg", 
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild4.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild5.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild6.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild7.jpg"
            };
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