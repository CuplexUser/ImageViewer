using System;
using System.Runtime.InteropServices;

namespace ImageViewer.Utility
{
    public class NativeMethods
    {
        internal const int WM_NCLBUTTONDOWN = 0xA1;
        internal static readonly IntPtr HT_CAPTION = new IntPtr(0x2);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();


        //[DllImport("user32.dll")]
        //internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        //[DllImport("user32.dll")]

        //internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}