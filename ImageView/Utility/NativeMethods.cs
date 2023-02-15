using System.Runtime.InteropServices;

namespace ImageViewer.Utility;

public class NativeMethods
{
    internal const int WM_NCLBUTTONDOWN = 0xA1;
    internal static readonly nint HT_CAPTION = new(0x2);

    [DllImport("user32.dll")]
    internal static extern nint SendMessage(nint hWnd, int Msg, nint wParam, nint lParam);

    [DllImport("user32.dll")]
    internal static extern bool ReleaseCapture();


    //[DllImport("user32.dll")]
    //internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    //[DllImport("user32.dll")]

    //internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
}