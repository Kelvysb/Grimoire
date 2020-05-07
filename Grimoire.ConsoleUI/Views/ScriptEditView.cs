using System;
using System.IO;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    internal class ScriptEditView : FrameView, IDisposable
    {
        public GrimoireScriptBlock ScriptBlock { get; set; }

        private Button btnSave;
        private Button btnCancel;

        private Label lblName;
        private TextField txtName;

        private Label lblDisplayName;
        private TextField txtDisplayName;

        private Label lblScriptFile;
        private TextField txtScriptFile;

        private Label lblExecutionType;
        private RadioGroup radExecute;

        private Label lblInterval;
        private TextField txtInterval;

        private Label lblTimeout;
        private TextField txtTimeout;

        private Label lblAlertLevel;
        private RadioGroup radAlertLevel;

        private Label lblSuccessPattern;
        private TextField txtSuccessPattern;

        private Label lblExtractBegin;
        private TextField txtExtractBegin;

        private Label lblExtractEnd;
        private TextField txtExtractEnd;

        public delegate void CancelHandler();

        public delegate void SaveHandler(GrimoireScriptBlock scriptBlock);

        public event CancelHandler Cancel;

        public event SaveHandler Save;

        public ScriptEditView(IGrimoireRunner scriptBlockRunner, int X) : base($"Edit - {scriptBlockRunner.ScriptBlock.Description}")
        {
            ScriptBlock = scriptBlockRunner.ScriptBlock;
            this.ColorScheme = Styles.DetailsScheme;
            this.X = X;
            Y = 0;
            Height = Dim.Fill();
            Width = Dim.Fill();
            Draw();
        }

        public ScriptEditView(int X) : base("New Script")
        {
            ScriptBlock = new GrimoireScriptBlock()
            {
                Name = "",
                Description = "",
                ExecutionMode = ExecutionMode.Manual,
                Interval = 0,
                AlertLevel = AlertLevel.Error,
                ScriptType = ScriptType.PowerShell,
                SuccessPatern = "",
                TimeOut = 10,
                Script = "",
                Group = "",
                ExtractResult = new PatternRange() { Start = "", End = "" }
            };
            this.ColorScheme = Styles.DetailsScheme;
            this.X = X;
            Y = 0;
            Height = Dim.Fill();
            Width = Dim.Fill();
            Draw();
        }

        private void Draw()
        {
            DrawFields();
            DrawButtons();
        }

        private void DrawButtons()
        {
            btnSave = new Button("Save")
            {
                X = Pos.Right(this) - 64,
                Y = Pos.Bottom(this) - 4
            };

            btnSave.Clicked = () => SaveScript();

            Add(btnSave);

            btnCancel = new Button("Cancel")
            {
                X = Pos.Right(this) - 75,
                Y = Pos.Bottom(this) - 4
            };

            btnCancel.Clicked = () => Cancel?.Invoke();

            Add(btnCancel);
        }

        private void DrawFields()
        {
            int col = 18;
            int textSize = 40;
            int variance = 2;
            int line = 1;

            lblName = new Label(1, line, "Name:");
            Add(lblName);
            txtName = new TextField(col, line, textSize, ScriptBlock.Name);
            Add(txtName);

            line += variance;

            lblDisplayName = new Label(1, line, "Display Name:");
            Add(lblDisplayName);
            txtDisplayName = new TextField(col, line, textSize, ScriptBlock.Description);
            Add(txtDisplayName);

            line += variance;

            lblScriptFile = new Label(1, line, "Script File:");
            Add(lblScriptFile);
            txtScriptFile = new TextField(col, line, textSize, "");
            Add(txtScriptFile);

            line += variance;

            lblInterval = new Label(1, line, "Interval:");
            Add(lblInterval);
            txtInterval = new TextField(col, line, textSize, ScriptBlock.Interval.ToString());
            txtInterval.OnLeave += TxtInterval_OnLeave;
            Add(txtInterval);

            line += variance;

            lblTimeout = new Label(1, line, "Timeout:");
            Add(lblTimeout);
            txtTimeout = new TextField(col, line, textSize, ScriptBlock.TimeOut.ToString());
            txtTimeout.OnLeave += TxtTimeout_OnLeave;
            Add(txtTimeout);

            line += variance;

            lblSuccessPattern = new Label(1, line, "Success Pattern:");
            Add(lblSuccessPattern);
            txtSuccessPattern = new TextField(col, line, textSize, ScriptBlock.SuccessPatern);
            Add(txtSuccessPattern);

            line += variance;

            lblExtractBegin = new Label(1, line, "Extract Start:"); ;
            Add(lblExtractBegin);
            txtExtractBegin = new TextField(col, line, textSize, ScriptBlock.ExtractResult.Start);
            Add(txtExtractBegin);

            line += variance;

            lblExtractEnd = new Label(1, line, "Extract End:"); ;
            Add(lblExtractEnd);
            txtExtractEnd = new TextField(col, line, textSize, ScriptBlock.ExtractResult.End);
            Add(txtExtractEnd);

            line += variance;

            lblExecutionType = new Label(1, line, "Execution:");
            Add(lblExecutionType);
            radExecute = new RadioGroup(col, line, new[] { "Manual", "Run on Start", "Interval" }, 0);
            Add(radExecute);
            switch (ScriptBlock.ExecutionMode)
            {
                case ExecutionMode.RunOnStart:
                    radExecute.Selected = 1;
                    break;

                case ExecutionMode.Interval:
                    radExecute.Selected = 2;
                    break;

                default:
                    radExecute.Selected = 0;
                    break;
            }

            line += variance + 2;

            lblAlertLevel = new Label(1, line, "Alert Level:");
            Add(lblAlertLevel);
            radAlertLevel = new RadioGroup(col, line, new[] { "Error", "Warning", "Silent" }, 0);
            Add(radAlertLevel);
            switch (ScriptBlock.AlertLevel)
            {
                case AlertLevel.Error:
                    radAlertLevel.Selected = 0;
                    break;

                case AlertLevel.Warning:
                    radAlertLevel.Selected = 1;
                    break;

                default:
                    radAlertLevel.Selected = 2;
                    break;
            }
        }

        private void TxtTimeout_OnLeave(object sender, EventArgs e)
        {
            int output = 0;
            if (int.TryParse(((TextField)sender).Text.ToString(), out output))
            {
                ((TextField)sender).Text = output.ToString();
            }
            else
            {
                ((TextField)sender).Text = "10";
            }
        }

        private void TxtInterval_OnLeave(object sender, EventArgs e)
        {
            int output = 0;
            if (int.TryParse(((TextField)sender).Text.ToString(), out output))
            {
                ((TextField)sender).Text = output.ToString();
            }
            else
            {
                ((TextField)sender).Text = "0";
            }
        }

        private void SaveScript()
        {
            ScriptBlock.Name = txtName.Text.ToString();
            ScriptBlock.Description = txtDisplayName.Text.ToString();

            switch (radExecute.Selected)
            {
                case 0:
                    ScriptBlock.ExecutionMode = ExecutionMode.Manual;
                    break;

                case 1:
                    ScriptBlock.ExecutionMode = ExecutionMode.RunOnStart;
                    break;

                default:
                    ScriptBlock.ExecutionMode = ExecutionMode.Interval;
                    break;
            }
            ScriptBlock.Interval = int.Parse(txtInterval.Text.ToString());

            switch (radAlertLevel.Selected)
            {
                case 0:
                    ScriptBlock.AlertLevel = AlertLevel.Error;
                    break;

                case 1:
                    ScriptBlock.AlertLevel = AlertLevel.Warning;
                    break;

                default:
                    ScriptBlock.AlertLevel = AlertLevel.Silent;
                    break;
            }

            ScriptBlock.SuccessPatern = txtSuccessPattern.Text.ToString();
            ScriptBlock.TimeOut = int.Parse(txtTimeout.Text.ToString());

            ScriptBlock.ExtractResult = new PatternRange()
            {
                Start = txtExtractBegin.Text.ToString(),
                End = txtExtractEnd.Text.ToString()
            };

            if (LoadFile(txtScriptFile.Text.ToString()))
            {
                Save?.Invoke(ScriptBlock);
            }
        }

        private bool LoadFile(string filePath)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader file = new StreamReader(filePath))
                    {
                        ScriptBlock.OriginalScriptFile = new MemoryStream();
                        file.BaseStream.CopyTo(ScriptBlock.OriginalScriptFile);
                        ScriptBlock.Script = Path.GetFileName(filePath);
                    }
                }
                else
                {
                    MessageBox.ErrorQuery(20, 7, "Grimoire", "File not found.", new[] { "Ok" });
                    result = false;
                }
            }
            return result;
        }

        public void Dispose()
        {
            btnSave.Clicked = null;
            btnCancel.Clicked = null;
        }
    }
}