using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class ResultView : FrameView
    {
        private string result;
        private ScrollView scrollView;
        private TextField textField;

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
            this.result = result;
            Draw();
        }

        private void Draw()
        {
            textField = new TextField(result)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
        }
    }
}