using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using NStack;
using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    public class SideListView : FrameView
    {
        private const int REFRESH_TIME = 1000;

        public List<IGrimoireRunner> scriptRunners = new List<IGrimoireRunner>();

        public delegate void ScriptSelectionHandler(IGrimoireRunner script);

        public event ScriptSelectionHandler Selected;

        private ListView Items;

        public SideListView(ustring title) : base(title)
        {
            RefreshTimer();
        }

        private void Draw()
        {
            if (Items != null)
            {
                Items.SelectedChanged -= () => SelectScript(scriptRunners[Items.SelectedItem]);
                Remove(Items);
            }
            Items = new ListView(scriptRunners)
            {
                X = 0,
                Y = 0,
                Width = Width - 2,
                Height = Height - 3
            };
            Add(Items);
            Items.SelectedChanged += () => SelectScript(scriptRunners[Items.SelectedItem]);
        }

        public void LoadScripts(List<IGrimoireRunner> scriptRunners)
        {
            this.scriptRunners = scriptRunners;
            Draw();
        }

        private void SelectScript(IGrimoireRunner script)
        {
            scriptRunners.ForEach(item => item.Selected = false);
            script.Selected = true;
            Selected?.Invoke(script);
        }

        private async void RefreshTimer()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    SetNeedsDisplay();
                    Thread.Sleep(REFRESH_TIME);
                }
            });
        }
    }
}