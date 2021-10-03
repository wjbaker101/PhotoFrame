using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace PhotoFrame.Windows.Main.Models
{
    public class ImageDetailsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private string _dimensionsLabel;
        public string DimensionsLabel
        {
            get => _dimensionsLabel;
            set
            {
                _dimensionsLabel = value;
                OnPropertyChanged(nameof(DimensionsLabel));
            }
        }

        private string _fileSizeLabelLabel;
        public string FileSizeLabel
        {
            get => _fileSizeLabelLabel;
            set
            {
                _fileSizeLabelLabel = value;
                OnPropertyChanged(nameof(FileSizeLabel));
            }
        }
    }
}