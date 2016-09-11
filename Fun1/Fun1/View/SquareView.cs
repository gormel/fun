using System;
using System.Windows.Media;
using Fun1.Model;

namespace Fun1.View
{
    public class SquareView
    {
        public int Width { get; set; } = 20;
        public int Height { get; set; } = 20;

        public int ScreenX => mX * Width;
        public int ScreenY => mY * Height;

        public int FieldX => mX;
        public int FieldY => mY;

        private readonly FieldView mFieldView;
        private readonly Square mSquare;
        private readonly int mX;
        private readonly int mY;

        public Brush Color => mFieldView.Palette.Colors[mSquare.Color];

        public event Action<SquareView> OnClick;

        public SquareView(FieldView fieldView, Square square, int x, int y)
        {
            mX = x;
            mY = y;
            mFieldView = fieldView;
            mSquare = square;
        }

        public virtual void CallOnClick()
        {
            OnClick?.Invoke(this);
        }
    }
}
