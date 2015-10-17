using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class AccessRightDefinition
    {

        public string Name { get; set; }

        public string Description { get; set; }
    }
}