using Photolemur.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Photolemur
{
    //https://habrahabr.ru/post/338518/
    public class MainVM : BindableBase
    {
        
        readonly MainModel _model = new MainModel();

        public MainVM()
        {
            _model.PropertyChanged += (s, e) =>
            {
                RaisePropertyChanged(e.PropertyName);
            };

            #region Images_LoadCommand
            Images_LoadCommand = new DelegateCommand<string>(path => { _model.ImageCollection_Load(path); SingleViewMode = false; });
            #endregion
            #region Images_DropCommand
            Images_DropCommand = new DelegateCommand<DragEventArgs>(dragEventArgs => _model.ImageCollection_Load(dragEventArgs));
            #endregion

            #region SingleViewModeOnCommand
            SingleViewModeOnCommand = new DelegateCommand<ImageItem>(imageItem => { CurrentImage = imageItem; SingleViewMode = true; });
            #endregion

            #region EscPressedCommand
            EscPressedCommand = new DelegateCommand(() => SingleViewMode = false, () => SingleViewMode);
            #endregion

            #region Image_NextCommand
            Image_NextCommand = new DelegateCommand(
                () => CurrentImage = _model.Image_Next(CurrentImage), 
                () => !Image_CommandsBlock);
            #endregion
            #region Image_PreviousCommand
            Image_PreviousCommand = new DelegateCommand(
                () => CurrentImage = _model.Image_Previous(CurrentImage), 
                () => !Image_CommandsBlock);
            #endregion
            #region Image_BlurCommand
            Image_BlurCommand = new DelegateCommand(
                () =>
                {
                    UIServices.SetBusyState();
                    try
                    {                        
                        _model.Image_Blur(CurrentImage);                        
                    }
                    catch (Exception ex) { }
                }, 
                () => !Image_CommandsBlock);
            #endregion

            CurrentImage = null;
            SingleViewMode = false;
            Image_CommandsBlock = true;
        }


        public DelegateCommand<string> Images_LoadCommand { get; }
        public DelegateCommand<DragEventArgs> Images_DropCommand { get; }
        public DelegateCommand<ImageItem> SingleViewModeOnCommand { get; }
        public DelegateCommand EscPressedCommand { get; }

        private bool _image_CommandsBlock;
        public bool Image_CommandsBlock
        {
            get
            {
                return _image_CommandsBlock;
            }

            set
            {
                _image_CommandsBlock = value;
                RaisePropertyChanged();
                Image_NextCommand.RaiseCanExecuteChanged();
                Image_PreviousCommand.RaiseCanExecuteChanged();
                Image_BlurCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand Image_NextCommand { get; }
        public DelegateCommand Image_PreviousCommand { get; }
        public DelegateCommand Image_BlurCommand { get; }        

        public ReadOnlyObservableCollection<ImageItem> imageCollection => _model.ImageCollection;

        private bool _singleViewMode;
        public bool SingleViewMode
        {
            get
            {
                return _singleViewMode;
            }

            set
            {
                _singleViewMode = value;
                Image_CommandsBlock = !value;
                RaisePropertyChanged();
                EscPressedCommand.RaiseCanExecuteChanged();
            }
        }

        private ImageItem _currentImage;
        public ImageItem CurrentImage
        {
            get
            {
                return _currentImage;
            }

            set
            {
                _currentImage = value;
                RaisePropertyChanged();
            }
        }        
    }
}
