using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class AboutBase : ComponentBase
    {
        public string Version { get; set; }

        protected override Task OnInitializedAsync()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            Version = fvi.FileVersion;
            return base.OnInitializedAsync();
        }
    }
}