using System;
using Grimoire.ConsoleUI.Helpers;
using Grimoire.ConsoleUI.Views;
using Grimoire.Domain.Models;
using Terminal.Gui;

namespace Grimoire.ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;
            top.ColorScheme = Styles.MainWindowScheme;

            // Creates the top-level window to show
            var win = new Window("Grimoire")
            {
                X = 0,
                Y = 1,
                ColorScheme = Styles.MainWindowScheme,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);

            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New Script", "", () => NewScript()),
                new MenuItem ("_Close", "", () => top.Running = false)
                })
            });
            menu.ColorScheme = Styles.MenuScheme;

            top.Add(menu);

            FrameView SideList = new FrameView("Scripts")
            {
                X = 0,
                Y = 0,
                Width = 50,
                Height = Dim.Fill(),
                ColorScheme = Styles.ListScheme
            };

            win.Add(SideList);

            GrimoireScriptBlock scriptBlock = new GrimoireScriptBlock()
            {
                Description = "Teste"
            };

            ScriptDetailView detail = new ScriptDetailView(scriptBlock, 51);

            detail.Click += EventRunTest;

            win.Add(detail);

            Application.Run();
        }

        private static void EventRunTest(object sender, ScriptClickEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.ScriptBlock.Description);
        }

        private static void NewScript()
        {
        }
    }
}