using System.Collections.Generic;
using System.Drawing;
using SuperJson.Parser;

namespace VectorEditor
{
    public class CircleShape : Shape
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int R { get; private set; }
        
        EditPoint mP1;
        EditPoint mP2;

        public CircleShape(int x, int y, int r)
        {
            X = x;
            Y = y;
            R = r;

            mP1 = new EditPoint(X, Y);
            mP1.PointChanged += OnPointChanged;
            mP2 = new EditPoint(X + R, Y);
            mP2.PointChanged += OnPointChanged;
        }

        private void OnPointChanged(EditPoint editPoint)
        {
            X = mP1.X;
            Y = mP1.Y;
            R = (int) mP1.Point.DistanceTo(mP2.Point);
        }

        public override void Render(Graphics g)
        {
            g.DrawEllipse(new Pen(Color.Black, 3), X - R, Y - R, R * 2, R * 2);
        }

        public override List<EditPoint> EditPoints()
        {
            return new List<EditPoint> { mP1, mP2 };
        }
    }
}
