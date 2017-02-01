using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorEditor
{
    public class LineShape : Shape
    {
        public Color Color { get; set; } = Color.Black;

        public int X1 { get; private set; }
        public int Y1 { get; private set; }

        public int X2 { get; private set; }
        public int Y2 { get; private set; }

        private readonly List<EditPoint>  mEditPoints = new List<EditPoint>();

        public LineShape(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            var p1 = new EditPoint(X1, Y1);
            p1.PointChanged += P1OnPointChanged;
            mEditPoints.Add(p1);

            var p2 = new EditPoint(X2, Y2);
            p2.PointChanged += P2OnPointChanged;
            mEditPoints.Add(p2);
        }

        private void P2OnPointChanged(EditPoint editPoint)
        {
            X2 = editPoint.X;
            Y2 = editPoint.Y;
        }

        private void P1OnPointChanged(EditPoint p)
        {
            X1 = p.X;
            Y1 = p.Y;
        }

        public override void Render(Graphics g)
        {
            g.DrawLine(new Pen(Color, 3), X1, Y1, X2, Y2);
        }

        public override List<EditPoint> EditPoints()
        {
            return mEditPoints;
        }
    }
}
