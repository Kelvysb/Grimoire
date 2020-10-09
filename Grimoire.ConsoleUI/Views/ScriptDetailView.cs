using System;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class ScriptDetailView : FrameView, IDisposable
    {
        public IGrimoireRunner ScriptBlockRunner { get; set; }

        private Button btnRun;

        private Button btnEdit;

        private Label lblLastResultDesc;
        private Label lblLastResult;

        private Label lblTypeDesc;
        private Label lblType;

        private Label lblStatusDesc;
        private Label lblStatus;

        private ResultView resultBlock;
        private ResultView warningBlock;
        private ResultView ErrorBlock;

        public delegate void EditHandler(IGrimoireRunner script);

        public event EditHandler Edit;

        public ScriptDetailView(IGrimoireRunner scriptBlockRunner, int X) : base(scriptBlockRunner.ScriptBlock.Description)
        {
            ScriptBlockRunner = scriptBlockRunner;
            ScriptBlockRunner.Finish += FinishRun;
            this.ColorScheme = Styles.DetailsScheme;
            this.X = X;
            Y = 0;
            Height = Dim.Fill();
            Width = Dim.Fill();
            Draw();
        }

        private void Draw()
        {
            DrawHeader();
            DrawResults();
            DrawButtons();
            UpdateResults();
        }

        private void DrawButtons()
        {
            btnRun = new Button("Run", true)
            {
                X = Pos.Right(this) - 64,
                Y = Pos.Bottom(this) - 4
            };

            btnRun.Clicked = () => Run();

            Add(btnRun);

            btnEdit = new Button("Edit", true)
            {
                X = Pos.Right(this) - 75,
                Y = Pos.Bottom(this) - 4
            };

            btnEdit.Clicked = () => Edit?.Invoke(ScriptBlockRunner);

            Add(btnEdit);
        }

        private void DrawResults()
        {
            resultBlock = new ResultView("Result", ScriptBlockRunner.ScriptBlock.LastResult?.FilteredResult)
            {
                X = 0,
                Y = Pos.Percent(0) + 2,
                Width = this.Width,
                Height = Dim.Percent(30)
            };
            Add(resultBlock);

            warningBlock = new ResultView("Warnings", ScriptBlockRunner.ScriptBlock.LastResult?.Warninings)
            {
                X = 0,
                Y = Pos.Percent(30) + 1,
                Width = this.Width,
                Height = Dim.Percent(45)
            };
            Add(warningBlock);

            ErrorBlock = new ResultView("Errors", ScriptBlockRunner.ScriptBlock.LastResult?.Errors)
            {
                X = 0,
                Y = Pos.Percent(60) + 1,
                Width = this.Width,
                Height = Dim.Percent(90)
            };
            Add(ErrorBlock);
        }

        private void DrawHeader()
        {
            lblStatusDesc = new Label(1, 0, "Status:");
            Add(lblStatusDesc);
            lblStatus = new Label(10, 0, ScriptBlockRunner.IsRunning ? "Running" : "Stopped");
            Add(lblStatus);

            lblLastResultDesc = new Label(19, 0, "Last Result:");
            Add(lblLastResultDesc);
            lblLastResult = new Label(32, 0, "N/A");
            if (ScriptBlockRunner.ScriptBlock.LastResult != null)
            {
                switch (ScriptBlockRunner.ScriptBlock.LastResult.ResultType)
                {
                    case ResultType.Success:
                        lblLastResult.Text = "Success";
                        break;

                    case ResultType.Error:
                        lblLastResult.Text = "Error";
                        break;

                    case ResultType.Warning:
                        lblLastResult.Text = "Warning";
                        break;

                    default:
                        lblLastResult.Text = "N/A";
                        break;
                }
            }
            Add(lblLastResult);

            lblTypeDesc = new Label(1, 1, "Running Type:");
            Add(lblTypeDesc);
            lblType = new Label(16, 1, "");
            switch (ScriptBlockRunner.ScriptBlock.ExecutionMode)
            {
                case ExecutionMode.RunOnStart:
                    lblType.Text = "Run on start";
                    break;

                case ExecutionMode.Interval:
                    lblType.Text = "Interval - " + ScriptBlockRunner.ScriptBlock.Interval + " seconds";
                    break;

                case ExecutionMode.Manual:
                    lblType.Text = "Manual";
                    break;
            }
            Add(lblType);
        }

        private void UpdateResults()
        {
            if (ScriptBlockRunner.ScriptBlock.LastResult != null)
            {
                if (resultBlock != null)
                {
                    resultBlock.Result = ScriptBlockRunner.ScriptBlock.LastResult.FilteredResult;
                }
                if (warningBlock != null)
                {
                    warningBlock.Result = ScriptBlockRunner.ScriptBlock.LastResult.Warninings;
                }
                if (ErrorBlock != null)
                {
                    ErrorBlock.Result = ScriptBlockRunner.ScriptBlock.LastResult.Errors;
                }
            }
        }

        private void Run()
        {
            if (!ScriptBlockRunner.IsRunning)
            {
                lblStatus.Text = "Running";
                ScriptBlockRunner.Run(null);
            }
        }

        private void FinishRun(object sender, ScriptResult result)
        {
            lblStatus.Text = "Stopped";
            UpdateResults();
        }

        public void Dispose()
        {
            btnRun.Clicked = null;
            btnEdit.Clicked = null;
        }
    }
}