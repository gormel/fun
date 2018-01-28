using System;
using System.Linq;

namespace FunMath.MultiTree
{
    public struct MultiVector
    {
        private readonly double[] mValues;

        public double LenghtSq => Dot(this);
        public double Lenght => Math.Sqrt(LenghtSq);
        public int Dimentions => mValues.Length;
        
        public MultiVector(double[] values, bool copy = true)
        {
            if (copy)
                values = Array.ConvertAll(values, d => d);
            mValues = values;
        }

        public double this[int index] => mValues[index];
        public static MultiVector operator +(MultiVector a, MultiVector b) => 
            new MultiVector(a.mValues.Select((d, i) => d + b[i]).ToArray(), false);
        public static MultiVector operator -(MultiVector a) =>
            new MultiVector(a.mValues.Select(d => -d).ToArray(), false);
        public static MultiVector operator *(MultiVector a, double b) =>
            new MultiVector(a.mValues.Select(d => d * b).ToArray(), false);

        public static MultiVector operator -(MultiVector a, MultiVector b) => a + -b;
        public static MultiVector operator +(MultiVector a) => - -a;
        public static MultiVector operator *(double a, MultiVector b) => b * a;
        public static MultiVector operator /(MultiVector a, double b) => a * (1 / b);

        public static bool operator >(MultiVector a, MultiVector b)
            => a.mValues.Select((d, i) => d > b[i]).All(more => more);
        public static bool operator <(MultiVector a, MultiVector b) => !(a > b);

        public double Dot(MultiVector b) => mValues.Select((d, i) => d * b[i]).Sum();
    }
}
