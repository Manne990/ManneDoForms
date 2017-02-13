namespace ManneDoForms.Common.PhotoViewer.Model
{
    public class ImageViewModel : BaseViewModel
    {
        private string _imageName;
        public string ImageName 
        {
            get 
            {
                return _imageName;
            }
            set 
            {
                SetObservableProperty(ref _imageName, value);
            }
        }

        private string _imageUrl;
        public string ImageUrl 
        {
            get 
            {
                return _imageUrl;
            }
            set 
            {
                SetObservableProperty(ref _imageUrl, value);
            }
        }
    }
}