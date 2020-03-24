using Grimoire.Domain.Abstraction.Business;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class IndexBase : ComponentBase
    {
        private IGrimoireBusiness grimoireBusiness;

        public IndexBase(IGrimoireBusiness grimoireBusiness)
        {
            this.grimoireBusiness = grimoireBusiness;
        }
    }
}