using Microsoft.Win32;
using PhotoFrame.Helpers;
using PhotoFrame.Utils;
using PhotoFrame.Windows.Main.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XamlAnimatedGif;

namespace PhotoFrame.Windows.Main
{
    public partial class MainWindow : Window
    {
        private FileInfo _imageFileInfo;
        private double _imageContainerWidth;
        private double _imageContainerHeight;
        private double _imageZoomModifier;

        private bool _isImageDragging;
        private double _imageDragStartX;
        private double _imageDragStartY;
        private double _imageOffsetX;
        private double _imageOffsetY;

        private BitmapImage _displayedImage;

        private readonly MainWindowModel _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            _dataContext = new MainWindowModel();
            DataContext = _dataContext;

            _imageZoomModifier = 1.0;
            _isImageDragging = false;
            _imageOffsetX = 0.0;
            _imageOffsetY = 0.0;

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                var openedFile = Environment.GetCommandLineArgs()[1];
                SetBitmapImage(openedFile);
            }
        }

        private void OnOpenImage(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = ImageHelper.GetOpenImageDialogFilter(),
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() != true)
                return;

            SetBitmapImage(openFileDialog.FileName);
        }

        private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _imageContainerWidth = e.NewSize.Width;
            _imageContainerHeight = e.NewSize.Height;

            UpdateImageSize();
        }

        private void SetBitmapImage(string fileName)
        {
            if (!Uri.TryCreate(fileName, UriKind.Absolute, out var fileUri))
            {
                _dataContext.CurrentMainScreen = CurrentMainScreenType.INVALID_IMAGE;
                return;
            }

            _imageFileInfo = new FileInfo(fileUri.LocalPath);
            if (!_imageFileInfo.Exists)
            {
                _dataContext.CurrentMainScreen = CurrentMainScreenType.INVALID_IMAGE;
                return;
            }

            try
            {
                _dataContext.GifControls.IsVisible = Visibility.Hidden;
                _displayedImage = new BitmapImage(fileUri);

                if (_imageFileInfo.Extension == ".gif")
                {
                    AnimationBehavior.SetSourceUri(DisplayedImage, fileUri);
                    _dataContext.GifControls.IsVisible = Visibility.Visible;
                }
                else
                {
                    DisplayedImage.Source = _displayedImage;
                }
            }
            catch
            {
                _dataContext.CurrentMainScreen = CurrentMainScreenType.INVALID_IMAGE;
                return;
            }

            UpdateImageSize();
            
            _dataContext.WindowTitle = $"PhotoFrame ({_imageFileInfo.Name})";
            
            _dataContext.CurrentMainScreen = CurrentMainScreenType.DISPLAYED_IMAGE;
            _dataContext.ImageDetails.DimensionsLabel = $"{_displayedImage.PixelWidth}x{_displayedImage.PixelHeight}";
            _dataContext.ImageDetails.FileSizeLabel = FileUtils.FormatFileSize(_imageFileInfo.Length);
        }

        private void UpdateImageSize()
        {
            if (_displayedImage == null)
                return;

            var (newWidth, newHeight) = ImageHelper.FitWithinBounds(
                _displayedImage.PixelWidth,
                _displayedImage.PixelHeight,
                _imageContainerWidth,
                _imageContainerHeight);

            _imageOffsetX = 0.0;
            _imageOffsetY = 0.0;
            DisplayedImage.RenderTransform = new TranslateTransform(_imageOffsetX, _imageOffsetY);

            DisplayedImage.Width = Math.Max(1, newWidth * _imageZoomModifier);
            DisplayedImage.Height = Math.Max(1, newHeight * _imageZoomModifier);
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnOpenInFileExplorer(object sender, RoutedEventArgs e)
        {
            if (_imageFileInfo?.Directory == null)
                return;

            try
            {
                Process.Start("explorer.exe", _imageFileInfo.Directory.FullName);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void OnCopyImage(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(_displayedImage);
        }

        private void OnCopyFileName(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_imageFileInfo.Name);
        }

        private void OnCopyFilePath(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_imageFileInfo.FullName);
        }

        private void OnZoomChanged(object sender, MouseWheelEventArgs e)
        {
            if (_dataContext.CurrentMainScreen != CurrentMainScreenType.DISPLAYED_IMAGE)
                return;

            if (e.Delta > 0)
            {
                _imageZoomModifier += 0.1;
            }
            else if (e.Delta < 0)
            {
                _imageZoomModifier -= 0.1;
            }

            UpdateImageSize();
        }

        private void OnToggleGifPlaying(object sender, RoutedEventArgs e)
        {
            _dataContext.GifControls.IsPlaying = !_dataContext.GifControls.IsPlaying;

            var gifController = AnimationBehavior.GetAnimator(DisplayedImage);

            if (_dataContext.GifControls.IsPlaying)
                gifController.Play();
            else
                gifController.Pause();
        }

        private void OnImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            _imageDragStartX = e.GetPosition(DisplayedImage).X;
            _imageDragStartY = e.GetPosition(DisplayedImage).Y;
            _isImageDragging = true;
        }

        private void OnImageMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            if (!_isImageDragging)
                return;

            _imageOffsetX -= _imageDragStartX - e.GetPosition(DisplayedImage).X;
            _imageOffsetY -= _imageDragStartY - e.GetPosition(DisplayedImage).Y;

            DisplayedImage.RenderTransform = new TranslateTransform(_imageOffsetX, _imageOffsetY);
        }

        private void OnImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isImageDragging = false;
        }

        private void OnImageMouseLeave(object sender, MouseEventArgs e)
        {
            _isImageDragging = false;
        }
    }
}