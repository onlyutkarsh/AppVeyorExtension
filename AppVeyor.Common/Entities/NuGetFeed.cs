using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class NuGetFeed
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool PublishingEnabled { get; set; }
        public string Created { get; set; }
    }
}