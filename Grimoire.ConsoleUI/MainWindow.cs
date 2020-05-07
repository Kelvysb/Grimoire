using System;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.ConsoleUI.Views;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Terminal.Gui;

namespace Grimoire.ConsoleUI
{
    internal class MainWindow
    {
        private IGrimoireBusiness business;
        private Toplevel Top;
        private Window Win;
        private MenuBar Menu;
        private SideListView SideList;
        private ScriptDetailView details;
        private ScriptEditView edit;

        public MainWindow(IGrimoireBusiness business)
        {
            this.business = business;
        }

        public void Initialize()
        {
            Application.Init();
            InitilizeWindow();
            InitilizeMenu();
            InitilizeSideList();
            Application.Run();
        }

        private void InitilizeSideList()
        {
            SideList = new SideListView("Scripts")
            {
                X = 0,
                Y = 0,
                Width = 50,
                Height = Dim.Fill(),
                ColorScheme = Styles.ListScheme
            };
            SideList.Selected += SelectScript;
            Win.Add(SideList);
            LoadScripts();
        }

        private void InitilizeMenu()
        {
            Menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New Script", "", () => NewScript()),
                new MenuItem ("_Close", "", () => Top.Running = false)
                })
            });
            Menu.ColorScheme = Styles.MenuScheme;
            Top.Add(Menu);
            Menu.SetNeedsDisplay();
        }

        private void InitilizeWindow()
        {
            Top = Application.Top;
            Top.ColorScheme = Styles.MainWindowScheme;
            Win = new Window("Grimoire")
            {
                X = 0,
                Y = 1,
                ColorScheme = Styles.MainWindowScheme,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            Top.Add(Win);
        }

        private async void LoadScripts()
        {
            await business.LoadScriptRunners();
            await Task.Run(() => SideList.LoadScripts(business.ScriptRunners?.ToList()));
        }

        private void SelectScript(IGrimoireRunner scriptRunner)
        {
            if (details != null)
            {
                Win.Remove(details);
                details.Edit -= EditScript;
                details.Dispose();
            }
            details = new ScriptDetailView(scriptRunner, 51);
            details.Edit += EditScript;
            Win.Add(details);
        }

        private void EditScript(IGrimoireRunner scriptRunner)
        {
            ClearWindow();
            edit = new ScriptEditView(scriptRunner, 51);
            edit.Save += Save;
            edit.Cancel += Cancel;
            Win.Add(edit);
        }

        private void NewScript()
        {
            ClearWindow();
            edit = new ScriptEditView(51);
            edit.Save += Save;
            edit.Cancel += Cancel;
            Win.Add(edit);
        }

        private async void Save(GrimoireScriptBlock scriptBlock)
        {
            await business.SaveScriptBlock(scriptBlock);
            await Task.Run(() => EndEdit(scriptBlock));
        }

        private void Cancel()
        {
            ClearWindow();
        }

        private void EndEdit(GrimoireScriptBlock scriptBlock)
        {
            LoadScripts();
            IGrimoireRunner editedScript = business.ScriptRunners
                .FirstOrDefault(item => item.ScriptBlock.Name.Equals(scriptBlock.Name,
                                                                     StringComparison.InvariantCultureIgnoreCase));
            SelectScript(editedScript);
        }

        private void ClearWindow()
        {
            if (details != null)
            {
                Win.Remove(details);
                details.Edit -= EditScript;
                details.Dispose();
            }
            if (edit != null)
            {
                Win.Remove(edit);
                edit.Save -= Save;
                edit.Cancel -= Cancel;
                edit.Dispose();
            }
        }
    }
}