using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class BuildInfo
    {
        public Project Project { get; set; }

        public Build Build { get; set; }
    }
}
