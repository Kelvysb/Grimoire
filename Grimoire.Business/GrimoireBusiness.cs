using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Business
{
    public class GrimoireBusiness : IGrimoireBusiness
    {
        public IEnumerable<IGrimoireRunner> ScriptRunners { get; set; }

        private readonly IGrimoireService grimoireService;

        public GrimoireBusiness(IGrimoireService grimoireService)
        {
            this.grimoireService = grimoireService;
        }

        public async Task<List<GrimoireScriptBlock>> GetScriptBlocks()
        {
            return await Task.Run(() => grimoireService.GetScriptBlocks().ToList());
        }

        public async Task SaveScriptBlock(GrimoireScriptBlock scriptBlock)
        {
            await Task.Run(() => grimoireService.SaveScriptBlocks(scriptBlock));
        }

        public async Task RemoveScriptBlock(string name)
        {
            await Task.Run(() => grimoireService.RemoveScriptBlock(name));
        }

        public async Task ChangeScripBlockOrder(GrimoireScriptBlock scriptBlock, int orderShift)
        {
            await Task.Run(() => "");
        }

        public async Task<List<ExecutionGroup>> GetExecutionGroups()
        {
            return await Task.Run(() => grimoireService.GetExecutionGroups().ToList());
        }

        public async Task RemoveExecutionGroup(string name)
        {
            await Task.Run(() => grimoireService.RemoveExecutionGroup(name));
        }

        public async Task SaveExecutionGroup(ExecutionGroup executionGroup)
        {
            await Task.Run(() => grimoireService.SaveExecutionGroup(executionGroup));
        }

        public Task<GrimoireScriptBlock> GetScriptBlock(string name)
        {
            return Task.Run(() => grimoireService.GetScriptBlock(name));
        }

        public Task<string> getScriptFullPath(GrimoireScriptBlock scriptBlock)
        {
            return Task.Run(() => grimoireService.getScriptFullPath(scriptBlock));
        }

        public async Task LoadScriptRunners()
        {
            IEnumerable<GrimoireScriptBlock> scripts = await GetScriptBlocks();
            ScriptRunners = await Task.Run(() =>
            {
                return scripts
                    .Select<GrimoireScriptBlock, IGrimoireRunner>(s => new GrimoireRunner(s, this))
                    .ToList();
            });
        }
    }
}