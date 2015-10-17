using System.Collections.Generic;
using System.Reflection;

namespace AppVeyor.Common.Entities
{

    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class Project
    {
        public int ProjectId { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public List<Build> Builds { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string RepositoryType { get; set; }

        public string RepositoryScm { get; set; }

        public string RepositoryName { get; set; }

        public string RepositoryBranch { get; set; }

        public bool IsPrivate { get; set; }

        public bool SkipBranchesWithoutAppveyorYml { get; set; }

        public NuGetFeed NuGetFeed { get; set; }

        public string Created { get; set; }

        public SecurityDescriptor SecurityDescriptor { get; set; }


    }
}
