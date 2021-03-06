﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Interfaces;

namespace Client
{
    public class User : INotifyPropertyChanged
    {
        private readonly Field mField;
        public int Width => mField.CellWidth;
        public int Height => mField.CellHeight;

        private string mNick = "";
        private int mX = 0;
        private int mY = 0;
        private float mDirection = 0;
        private string mSkinImage = "GrayTank.png";

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

        public string SkinImage
        {
            get { return mSkinImage; }
            set
            {
                mSkinImage = value;
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

        public User(Field field)
        {
            mField = field;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
