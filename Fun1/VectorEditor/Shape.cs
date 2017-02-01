using System;
using System.Collections.Generic;
using System.Drawing;

namespace VectorEditor
{
    public abstract class Shape
    {
        public abstract void Render(Graphics g);
        public abstract List<EditPoint> EditPoints();

        const int DefaultSize = 50;

        public static Shape CreateShape(ShapeType type, int x, int y)
        {
            if (type == ShapeType.Line)
            {
                return new LineShape(x, y, x + DefaultSize, y);
            }

            if (type == ShapeType.Circle)
            {
                return new CircleShape(x, y, DefaultSize);
            }

            if (type == ShapeType.Square)
            {
                return new SquareShape(x, y, DefaultSize * 2);
            }

            if (type == ShapeType.Triangle)
            {
                var d = (int)(DefaultSize / Math.Sqrt(2));
                return new TriangleShape(x - d, y + d, x, y - DefaultSize, x + d, y + d);
            }

            return null;
        }
    }
}
