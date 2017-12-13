using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Wpf_ImagesList
{
    public class ViewItem
    {
        public ViewItem(string title, BitmapImage image)
        {
            Title = title;
            ImageData = image;
        }
        public ViewItem(BitmapImage image, string title) : this(title, image) { }        

        public BitmapImage ImageData { get; set; }
        public string Title { get; set; }
    }
}
