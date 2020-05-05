using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class ResultView : FrameView
    {
        private string result;
        private ListView Text;

        public string Result
        {
            get => result;
            set
            {
                result = value;
                Draw();
            }
        }

        public ResultView(string title, string result) : base(title)
        {
            this.result = !string.IsNullOrEmpty(result) ? result : string.Empty;
            Draw();
        }

        private void Draw()
        {
            Text = new ListView(result.Split("\n"))
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            Add(Text);
        }
    }
}