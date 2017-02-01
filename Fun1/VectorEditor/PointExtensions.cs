using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorEditor
{
    public static class PointExtensions
    {
        public static float DistanceTo(this Point pt, Point p)
        {
            return (float)Math.Sqrt((pt.X - p.X)*(pt.X - p.X) + (pt.Y - p.Y)*(pt.Y - p.Y));
        }
    }
}
