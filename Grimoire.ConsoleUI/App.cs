using Grimoire.Domain.Abstraction.Business;

namespace Grimoire.ConsoleUI
{
    public class App
    {
        private IGrimoireBusiness business;

        public App(IGrimoireBusiness business)
        {
            this.business = business;
        }

        public void Run()
        {
            MainWindow mainWindow = new MainWindow(business);
            mainWindow.Initialize();
        }
    }
}