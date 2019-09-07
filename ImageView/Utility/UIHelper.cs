using System.Collections.Generic;
using System.Drawing;

namespace ImageViewer.Utility
{
    public static class UIHelper
    {
        public static List<Color> GetSelectableSystemBackgroundColors()
        {
            var list = new List<Color>
            {
                SystemColors.ActiveBorder,
                SystemColors.Window,
                SystemColors.ScrollBar,
                SystemColors.MenuText,
                SystemColors.MenuHighlight,
                SystemColors.MenuBar,
                SystemColors.Menu,
                SystemColors.InfoText,
                SystemColors.Info,
                SystemColors.InactiveCaptionText,
                SystemColors.InactiveCaption,
                SystemColors.InactiveBorder,
                SystemColors.HotTrack,
                SystemColors.HighlightText,
                SystemColors.Highlight,
                SystemColors.WindowFrame,
                SystemColors.GrayText,
                SystemColors.GradientActiveCaption,
                SystemColors.Desktop,
                SystemColors.ControlText,
                SystemColors.ControlLightLight,
                SystemColors.ControlLight,
                SystemColors.ControlDarkDark,
                SystemColors.ControlDark,
                SystemColors.Control,
                SystemColors.ButtonShadow,
                SystemColors.ButtonHighlight,
                SystemColors.ButtonFace,
                SystemColors.AppWorkspace,
                SystemColors.ActiveCaptionText,
                SystemColors.ActiveCaption,
                SystemColors.GradientInactiveCaption,
                SystemColors.WindowText,
            };

            return list;
        }

        public static List<Color> GetSelectableBackgroundColors()
        {
            var darkerGray = Color.FromArgb(255, 30, 30, 30);

            var list = new List<Color>
            {
                Color.AliceBlue,
                Color.Azure,
                Color.White,
                Color.WhiteSmoke,
                Color.Black,
                Color.Bisque,
                Color.BlanchedAlmond,
                Color.BurlyWood,
                Color.CadetBlue,
                Color.Cornsilk,
                Color.Gray,
                Color.Silver,
                Color.DarkGray,
                darkerGray,
                Color.DimGray,
                Color.SlateGray,
                Color.LightSlateGray,
                Color.PapayaWhip,
                Color.MidnightBlue,
                Color.SteelBlue,
                Color.LightSkyBlue,
            };

            return list;
        }
    }
}