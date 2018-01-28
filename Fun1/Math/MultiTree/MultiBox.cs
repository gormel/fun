using System;

namespace FunMath.MultiTree
{
    public struct MultiBox
    {
        public MultiVector Position { get; }
        public MultiVector Size { get; }

        public MultiBox(MultiVector position, MultiVector size)
        {
            Position = position;
            Size = size;
        }

        public bool Contains(MultiVector v)
        {
            return v > Position && v < Position + Size;
        }

        /// <summary>
        /// clculates intersection between two boxes
        /// </summary>
        /// <param name="a">checking box</param>
        /// <returns>intersection box, or null, if there are no intersection</returns>
        public MultiBox? Intersection(MultiBox a)
        {
            var pos = new double[Position.Dimentions];
            var siz = new double[Size.Dimentions];
            for (int i = 0; i < Position.Dimentions; i++)
            {
                var p = Math.Max(Position[i], a.Position[i]);
                var s = Math.Min(Position[i] + Size[i], a.Position[i] + a.Size[i]) - p;
                if (s < 0)
                    return null;
                pos[i] = p;
                siz[i] = s;
            }
            return new MultiBox(new MultiVector(pos, false), new MultiVector(siz, false));
        }
    }
}
