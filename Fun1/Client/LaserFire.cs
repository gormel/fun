using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace Client
{
    public class LaserFire : INotifyPropertyChanged
    {
        private int mX;
        private int mY;
        private int mX1;
        private int mY1;

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

        public int X1
        {
            get { return mX1; }
            set
            {
                mX1 = value;
                OnPropertyChanged();
            }
        }

        public int Y1
        {
            get { return mY1; }
            set
            {
                mY1 = value;
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
