using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class AccessRight
    {

        public string Name { get; set; }

        public bool Allowed { get; set; }
    }
}