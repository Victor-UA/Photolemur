using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_ImagesList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }



        public List<ViewItem> lstImages
        {
            get { return (List<ViewItem>)GetValue(lstImagesProperty); }
            set { SetValue(lstImagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lstImages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lstImagesProperty =
            DependencyProperty.Register("lstImages", typeof(List<ViewItem>), typeof(MainWindow), new PropertyMetadata(new List<ViewItem>()));


        private void btnLoadFolderPath_Click(object sender, RoutedEventArgs e)
        {
            lstImages.Clear();
            Image imgTemp;
            List<string> lstFileNames = new List<string>(System.IO.Directory.EnumerateFiles(txtFolderPath.Text, "*.jpg"));
            foreach (string fileName in lstFileNames)
            {
                imgTemp = new Image();
                imgTemp.Source = new BitmapImage(new Uri(fileName));
                imgTemp.Height = imgTemp.Width = 100;
                lstImages.Add(new ViewItem(fileName, new BitmapImage(new Uri(fileName))));                
            }
            lstView.ItemsSource = lstImages;
        }
    }
}
