using System.Reflection;
using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class AboutView : FrameView
    {
        private Button btnOk;
        private TextView textView;

        public delegate void OkHandler();

        public event OkHandler Ok;

        public AboutView(int X) : base("About")
        {
            this.ColorScheme = Styles.DetailsScheme;
            this.X = X;
            Y = 0;
            Height = Dim.Fill();
            Width = Dim.Fill();
            Draw();
        }

        private void Draw()
        {
            textView = new TextView()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1,
                ReadOnly = true
            };
            textView.Text = @$"Grimoire

Automation Script Helper.

Open source by Kelvys B.

Visit the project repository at https://github.com/Kelvysb/Grimoire

Version = {Assembly.GetEntryAssembly().GetName().Version}
";
            Add(textView);

            btnOk = new Button("Ok", true)
            {
                X = Pos.Right(this) - 64,
                Y = Pos.Bottom(this) - 4
            };
            btnOk.Clicked = () => Ok.Invoke();
            Add(btnOk);
        }
    }
}