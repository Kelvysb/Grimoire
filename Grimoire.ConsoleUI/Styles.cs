using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

namespace Grimoire.ConsoleUI
{
    internal class Styles
    {
        public static ColorScheme MainWindowScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Green, Color.Black),
            Disabled = Attribute.Make(Color.Gray, Color.Black),
            HotNormal = Attribute.Make(Color.BrightGreen, Color.Black),
            Focus = Attribute.Make(Color.Black, Color.Green),
            HotFocus = Attribute.Make(Color.DarkGray, Color.Green)
        };

        public static ColorScheme MenuScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Black, Color.Green),
            Disabled = Attribute.Make(Color.Black, Color.Gray),
            HotNormal = Attribute.Make(Color.Black, Color.BrightGreen),
            Focus = Attribute.Make(Color.Green, Color.Black),
            HotFocus = Attribute.Make(Color.Green, Color.DarkGray)
        };

        public static ColorScheme ListScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Green, Color.Black),
            Disabled = Attribute.Make(Color.Gray, Color.Black),
            HotNormal = Attribute.Make(Color.BrightGreen, Color.Black),
            Focus = Attribute.Make(Color.Black, Color.Green),
            HotFocus = Attribute.Make(Color.DarkGray, Color.Green)
        };

        public static ColorScheme DetailsScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Green, Color.Black),
            Disabled = Attribute.Make(Color.Gray, Color.Black),
            HotNormal = Attribute.Make(Color.BrightGreen, Color.Black),
            Focus = Attribute.Make(Color.Black, Color.Green),
            HotFocus = Attribute.Make(Color.DarkGray, Color.Green)
        };

        public static ColorScheme RunningScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Black, Color.BrightGreen),
            Disabled = Attribute.Make(Color.Gray, Color.BrightGreen),
            HotNormal = Attribute.Make(Color.Green, Color.BrightGreen),
            Focus = Attribute.Make(Color.BrightGreen, Color.Black),
            HotFocus = Attribute.Make(Color.BrightGreen, Color.Black)
        };

        public static ColorScheme SucessScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Black, Color.Green),
            Disabled = Attribute.Make(Color.Gray, Color.Green),
            HotNormal = Attribute.Make(Color.BrightGreen, Color.Green),
            Focus = Attribute.Make(Color.Green, Color.Black),
            HotFocus = Attribute.Make(Color.Green, Color.Black)
        };

        public static ColorScheme ErrorScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Black, Color.Red),
            Disabled = Attribute.Make(Color.Gray, Color.Red),
            HotNormal = Attribute.Make(Color.BrightRed, Color.Red),
            Focus = Attribute.Make(Color.Red, Color.Black),
            HotFocus = Attribute.Make(Color.Red, Color.Black)
        };

        public static ColorScheme WarningScheme = new ColorScheme()
        {
            Normal = Attribute.Make(Color.Black, Color.BrightYellow),
            Disabled = Attribute.Make(Color.Gray, Color.BrightYellow),
            HotNormal = Attribute.Make(Color.BrightYellow, Color.BrightYellow),
            Focus = Attribute.Make(Color.BrightYellow, Color.Black),
            HotFocus = Attribute.Make(Color.BrightYellow, Color.Black)
        };
    }
}