using System.Collections.Generic;
using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class Build
    {
        public int BuildId { get; set; }

        public List<Job> Jobs { get; set; }

        public int BuildNumber { get; set; }

        public string Version { get; set; }

        public string Message { get; set; }

        public string Branch { get; set; }

        public string CommitId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUsername { get; set; }

        public string CommitterName { get; set; }

        public string CommitterUsername { get; set; }

        public string Committed { get; set; }

        public List<string> Messages { get; set; }

        public List<string> Status { get; set; }

        public string Started { get; set; }

        public string Finished { get; set; }

        public string Created { get; set; }

        public string Updated { get; set; }
    }
}