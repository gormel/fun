using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fun1.Model
{
    public class Palette
    {
        public Palette(int colorCount)
        {
            Colors = new Brush[colorCount];
        }

        public Brush[] Colors { get; }

        public static Palette Default
        {
            get
            {
                var result = new Palette(5)
                {
                    Colors =
                    {
                        [0] = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                        [1] = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
                        [2] = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
                        [3] = new SolidColorBrush(Color.FromRgb(255, 255, 0)),
                        [4] = new SolidColorBrush(Color.FromRgb(255, 0, 255))
                    }
                };
                return result;
            }
        }
    }
}
