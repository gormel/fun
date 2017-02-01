
using System;
using System.Collections.Generic;
using System.Drawing;

namespace VectorEditor
{
    class TriangleShape : Shape
    {
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }
        public int X3 { get; private set; }
        public int Y3 { get; private set; }
        

        EditPoint mP1;
        EditPoint mP2;
        EditPoint mP3;

        public TriangleShape(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X3 = x3;
            Y3 = y3;

            mP1 = new EditPoint(X1, Y1);
            mP1.PointChanged += P1OnPointChanged;
            mP2 = new EditPoint(X2, Y2);
            mP2.PointChanged += P2OnPointChanged;
            mP3 = new EditPoint(X3, Y3);
            mP3.PointChanged += P3OnPointChanged;
        }

        private void P3OnPointChanged(EditPoint p)
        {
            X3 = p.X;
            Y3 = p.Y;
        }

        private void P2OnPointChanged(EditPoint p)
        {
            X2 = p.X;
            Y2 = p.Y;
        }

        private void P1OnPointChanged(EditPoint p)
        {
            X1 = p.X;
            Y1 = p.Y;
        }

        public override void Render(Graphics g)
        {
            g.DrawLine(new Pen(Color.Black, 3), X1, Y1, X2, Y2);
            g.DrawLine(new Pen(Color.Black, 3), X2, Y2, X3, Y3);
            g.DrawLine(new Pen(Color.Black, 3), X3, Y3, X1, Y1);
        }

        public override List<EditPoint> EditPoints()
        {
            return new List<EditPoint> { mP1, mP2, mP3 };
        }
    }
}
