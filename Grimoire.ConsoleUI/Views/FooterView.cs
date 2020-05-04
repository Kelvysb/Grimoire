using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class FooterView : View
    {
        public FooterView(Pos Y)
        {
            X = 0;
            this.Y = Y;
            Height = 1;
            Width = Dim.Fill();
            Draw();
        }

        private void Draw()
        {
            Label instructions = new Label(3, 0, "F9 - To access menu.");
            Add(instructions);
        }
    }
}