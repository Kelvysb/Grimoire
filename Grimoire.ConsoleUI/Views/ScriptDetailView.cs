using Grimoire.ConsoleUI.Helpers;
using Grimoire.Domain.Models;
using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class ScriptDetailView : FrameView
    {
        public GrimoireScriptBlock ScriptBlock { get; set; }

        private Button btnRun;

        private Button btnEdit;

        private Label lblTypeDesc;
        private Label lblType;
        private Label lblStatusDesc;
        private Label lblStatus;

        private Label lblResult;
        private ResultView resultBlock;

        private Label lblWarnings;
        private ResultView warningBlock;

        private Label lblErrors;
        private ResultView ErrorBlock;

        public delegate void ClickHandler(object sender, ScriptClickEventArgs eventArgs);

        public event ClickHandler Click;

        public ScriptDetailView(GrimoireScriptBlock scriptBlock, int X) : base(scriptBlock.Description)
        {
            ScriptBlock = scriptBlock;
            this.ColorScheme = Styles.DetailsScheme;
            this.X = X;
            Y = 0;
            Height = Dim.Fill();
            Width = Dim.Fill();
            Draw();
        }

        private void Draw()
        {
            lblStatusDesc = new Label(0, 0, "Status:");
            Add(lblStatusDesc);
            lblStatus = new Label(8, 0, "Running");
            Add(lblStatus);

            btnRun = new Button("Run", true)
            {
                X = Pos.Right(this) - 64,
                Y = Pos.Bottom(this) - 4
            };

            btnRun.Clicked = () => Click.Invoke(this, new ScriptClickEventArgs(ScriptBlock, ScriptActions.Run));

            Add(btnRun);

            btnEdit = new Button("Edit", true)
            {
                X = Pos.Right(this) - 75,
                Y = Pos.Bottom(this) - 4
            };

            btnRun.Clicked = () => Click.Invoke(this, new ScriptClickEventArgs(ScriptBlock, ScriptActions.Edit));

            Add(btnEdit);

            UpdateResults();
        }

        private void UpdateResults()
        {
            if (resultBlock != null)
            {
                resultBlock.Result = ScriptBlock.LastResult.FilteredResult;
            }
            if (warningBlock != null)
            {
                warningBlock.Result = ScriptBlock.LastResult.Warninings;
            }
            if (ErrorBlock != null)
            {
                ErrorBlock.Result = ScriptBlock.LastResult.Errors;
            }
        }
    }
}