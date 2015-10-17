using System.Collections.Generic;
using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class ProjectHistory
    {
        public Project Project { get; set; }

        public List<Build> Builds { get; set; }
    }
}
