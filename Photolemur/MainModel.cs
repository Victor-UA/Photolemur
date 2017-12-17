using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Linq;
using Photolemur.Utilities;

namespace Photolemur
{
    public class MainModel: BindableBase
    {
        const string EXTESIONS_PATTERN = "bmp$|jpg$|jpeg$|png$|ico$|gif$|tiff$";

        public MainModel()
        {
            _imageCollection = new ObservableCollection<ImageItem>();
            ImageCollection = new ReadOnlyObservableCollection<ImageItem>(_imageCollection);
        }

        private readonly ObservableCollection<ImageItem> _imageCollection;
        public readonly ReadOnlyObservableCollection<ImageItem> ImageCollection;

        internal void ImageCollection_Load(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ImageCollection_Add(files);
            }
            e.Handled = true;
        }

        public void ImageCollection_Add(string[] files)
        {
            IEnumerable<string> filteredFiles = MyDirectory.GetFiles(files, EXTESIONS_PATTERN);
            ImageCollection_Add(filteredFiles);
        }
        public void ImageCollection_Add(ImageItem item)
        {
            _imageCollection.Add(item);
        }
        private void ImageCollection_Add(IEnumerable<string> files, bool clear = false)
        {
            if (clear)
            {
                _imageCollection.Clear();
            }
            foreach (string file in files)
            {
                try
                {
                    if (!_imageCollection.Any(item => item.Image.UriSource.OriginalString == file))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(file);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        _imageCollection.Add(new ImageItem(Path.GetFileName(file), bitmap));                        
                    }
                }
                catch (Exception ex) { }
            }
        }

        public void Image_Blur(ImageItem imageItem)
        { 
            KalikoImage bluredImage = new KalikoImage(imageItem.UriSource.OriginalString);
            bluredImage.ApplyFilter(new GaussianBlurFilter());
            imageItem.Image = ImageToBitmapImage(bluredImage);
        }

        public void ImageCollection_Load(string path)
        {
            IEnumerable<string> files = MyDirectory.GetFiles(path, EXTESIONS_PATTERN);
            ImageCollection_Add(files, true);
        }               
        public void ImageCollection_Remove(ImageItem item)
        {
            if (_imageCollection.Contains(item))
            {
                _imageCollection.Remove(item);
            }
        }
        public void ImageCollection_RemoveAt(int index)
        {
            if (index >=0 && index < _imageCollection.Count)
            {
                _imageCollection.RemoveAt(index);
            }
        }

        internal ImageItem Image_Previous(ImageItem currentImage)
        {
            int index = ImageCollection.IndexOf(currentImage);
            index = --index >= 0 ? index : ImageCollection.Count - 1;
            return ImageCollection[index];
        }
        internal ImageItem Image_Next(ImageItem currentImage)
        {
            int index = ImageCollection.IndexOf(currentImage);
            index = ++index < ImageCollection.Count ? index : 0;
            return ImageCollection[index];
        }

        private BitmapImage ImageToBitmapImage(KalikoImage image)
        {
            using (var memory = new MemoryStream())
            {
                image.SavePng(memory, true);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;                
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

    }
}
