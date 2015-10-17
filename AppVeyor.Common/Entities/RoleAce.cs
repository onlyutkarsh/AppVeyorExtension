using System.Collections.Generic;
using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class RoleAce
    {

        public int RoleId { get; set; }

        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public List<AccessRight> AccessRights { get; set; }
    }
}