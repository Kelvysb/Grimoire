namespace Grimoire.Domain.Models
{
    public class ScriptResult
    {
        public ResultType ResultType { get; set; }

        public string FilteredResult { get; set; }

        public string RawResult { get; set; }

        public string Warninings { get; set; }

        public string Errors { get; set; }
    }
}