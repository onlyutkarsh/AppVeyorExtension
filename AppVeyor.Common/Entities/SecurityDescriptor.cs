using System.Collections.Generic;
using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class SecurityDescriptor
    {
        public List<AccessRightDefinition> AccessRightDefinitions { get; set; }

        public List<RoleAce> RoleAces { get; set; }
    }
}