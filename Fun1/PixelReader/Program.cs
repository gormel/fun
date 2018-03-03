using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelReader
{
    struct POINT
    {
        public int x;
        public int y;
    }
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetCursorPos(out POINT pt);

        static void Main(string[] args)
        {
            SerialPort serial = new SerialPort("COM3", 9600);
            serial.Open();
            serial.DataReceived += SerialOnDataReceived;
            while (true)
            {
                POINT cPos;
                if (!GetCursorPos(out cPos))
                    continue;
                var dc = GetWindowDC(GetDesktopWindow());
                var px = GetPixel(dc, cPos.x, cPos.y);
                Trace.WriteLine(px);

                serial.Write(BitConverter.GetBytes(px), 0, 4);
                Thread.Sleep(100);
            }
        }

        private static void SerialOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            var s = (SerialPort) sender;
            Console.WriteLine(s.ReadLine());
        }
    }
}
