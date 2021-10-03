using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PhotoFrame.Windows.Main.Models
{
    public struct CurrentMainScreenType
    {
        public const string DISPLAYED_IMAGE = "DisplayedImage";
        public const string CHOOSE_IMAGE = "ChooseImage";
        public const string INVALID_IMAGE = "InvalidImage";
    }

    public class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainWindowModel()
        {
            WindowTitle = "PhotoFrame";
            ImageDetails = new ImageDetailsModel();
            CurrentMainScreen = CurrentMainScreenType.CHOOSE_IMAGE;
            GifControls = new GifControlsModel();
        }

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        private ImageDetailsModel _imageDetails;
        public ImageDetailsModel ImageDetails
        {
            get => _imageDetails;
            set
            {
                _imageDetails = value;
                OnPropertyChanged(nameof(ImageDetails));
            }
        }

        private string _currentMainScreen;
        public string CurrentMainScreen
        {
            get => _currentMainScreen;
            set
            {
                _currentMainScreen = value;
                OnPropertyChanged(nameof(CurrentMainScreen));
            }
        }

        private GifControlsModel _gifControls;
        public GifControlsModel GifControls
        {
            get => _gifControls;
            set
            {
                _gifControls = value;
                OnPropertyChanged(nameof(GifControls));
            }
        }
    }
}