using System;
using System.Drawing;
using SuperJson.Parser;

namespace VectorEditor
{
    public class EditPoint
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        [DoNotSerialise]
        public Point Point => new Point(X, Y);

        public event Action<EditPoint> PointChanged;

        public EditPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveTo(int newX, int newY)
        {
            X = newX;
            Y = newY;

            PointChanged?.Invoke(this);
        }
    }
}
