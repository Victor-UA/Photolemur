using Prism.Mvvm;
using System;
using System.Windows.Media.Imaging;

namespace Photolemur
{
    public class ImageItem: BindableBase
    {
        public ImageItem(string title, BitmapImage image)
        {
            Title = title;
            Image = image;
            UriSource = Image.UriSource;
            Data = this;
        }
        public ImageItem(BitmapImage image, string title) : this(title, image) { }        

        public ImageItem Data { get; private set; }
        private BitmapImage _image;
        public BitmapImage Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                RaisePropertyChanged();
            }
        }
        public Uri UriSource { get; set; }
        public string Title { get; set; }

    }
}
