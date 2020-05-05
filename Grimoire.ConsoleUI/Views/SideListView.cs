using System.Collections.Generic;
using Grimoire.Domain.Abstraction.Business;
using NStack;
using Terminal.Gui;

namespace Grimoire.ConsoleUI.Views
{
    public class SideListView : FrameView
    {
        public List<IGrimoireRunner> scriptRunners = new List<IGrimoireRunner>();

        public delegate void ScriptSelectionHandler(IGrimoireRunner script);

        public event ScriptSelectionHandler Selected;

        private ListView Items;

        public SideListView(ustring title) : base(title)
        {
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
                Height = Height - 2
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
    }
}