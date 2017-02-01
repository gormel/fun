using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorEditor
{
    public class SquareShape : Shape
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Side { get; private set; }

        EditPoint mP1;
        EditPoint mP2;

        public SquareShape(int x, int y, int side)
        {
            X = x;
            Y = y;
            Side = side;

            mP1 = new EditPoint(X, Y);
            mP1.PointChanged += OnPointChanged;
            mP2 = new EditPoint(X + Side / 2, Y);
            mP2.PointChanged += OnPointChanged;
        }

        private void OnPointChanged(EditPoint editPoint)
        {
            X = mP1.X;
            Y = mP1.Y;
            Side = (int)mP1.Point.DistanceTo(mP2.Point) * 2;
        }

        public override void Render(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.Black, 3), X - Side / 2, Y - Side / 2, Side, Side);
        }

        public override List<EditPoint> EditPoints()
        {
            return new List<EditPoint> { mP1, mP2 };
        }
    }
}
