using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PhotoFrame.Windows.Main.Models
{
    public class GifControlsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public GifControlsModel()
        {
            IsVisible = Visibility.Hidden;
            IsPlaying = true;
        }

        private Visibility _isVisible;
        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }
    }
}