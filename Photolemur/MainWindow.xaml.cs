using System.Windows;
using System.Windows.Data;

namespace Photolemur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetBinding(SingleViewModeProperty, new Binding("SingleViewMode"));
            SetBinding(CurrentImageProperty, new Binding("CurrentImage"));
                       
            if (Properties.Settings.Default.Main_Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }
        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Main_Maximized = WindowState == WindowState.Maximized;            
            Properties.Settings.Default.Save();
        }

        public bool SingleViewMode
        {
            get { return (bool)GetValue(SingleViewModeProperty); }
            set { SetValue(SingleViewModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleImageView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleViewModeProperty =
            DependencyProperty.Register("SingleViewMode", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));




        public ImageItem CurrentImage
        {
            get { return (ImageItem)GetValue(CurrentImageProperty); }
            set { SetValue(CurrentImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentImageProperty =
            DependencyProperty.Register("CurrentImage", typeof(ImageItem), typeof(MainWindow), new PropertyMetadata(null));

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            MainVM vm = (MainVM)DataContext;
            if (vm.Images_DropCommand.CanExecute(e))
            {
                vm.Images_DropCommand.Execute(e);
            }
        }
        
    }
}