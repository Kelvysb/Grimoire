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
    }
}