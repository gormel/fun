using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Interfaces;

namespace Client
{
    public class User : INotifyPropertyChanged
    {
        public int Width { get; } = 30;
        public int Height { get; } = 30;

        private string mNick;
        private int mX;
        private int mY;
        private float mDirection;
        private Brush mColor;

        public int X
        {
            get { return mX; }
            set
            {
                mX = value;
                OnPropertyChanged();
            }
        }

        public int Y
        {
            get { return mY; }
            set
            {
                mY = value;
                OnPropertyChanged();
            }
        }

        public float Direction
        {
            get { return mDirection; }
            set
            {
                mDirection = value;
                OnPropertyChanged();
            }
        }

        public Brush Color
        {
            get { return mColor; }
            set
            {
                mColor = value;
                OnPropertyChanged();
            }
        }

        public string Nick
        {
            get { return mNick; }
            set
            {
                mNick = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
