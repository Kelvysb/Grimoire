using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class SideMenuBase : ComponentBase
    {
        public bool collapseNavMenu = true;

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}