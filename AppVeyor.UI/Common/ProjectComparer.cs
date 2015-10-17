using System;
using System.Collections.Generic;
using System.Linq;
using AppVeyor.Common.Entities;

namespace AppVeyor.UI.Common
{
    public class ProjectComparer : IEqualityComparer<Project>
    {
        public bool Equals(Project first, Project second)
        {
            if (ReferenceEquals(first, second))
               return true;

            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            var firstProjectBuild = first.Builds.First();
            var secondProjectBuild = second.Builds.First();

            var firstProjectBuildStatus = firstProjectBuild.Status.First();
            var secondProjectBuildStatus = secondProjectBuild.Status.First();

            if(firstProjectBuildStatus.Equals("queued", StringComparison.InvariantCultureIgnoreCase)
                || firstProjectBuildStatus.Equals("running", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var buildStausEqual = (firstProjectBuildStatus
                    .Equals(secondProjectBuildStatus, StringComparison.InvariantCultureIgnoreCase));

            var branchEqual = firstProjectBuild.Branch.Equals(secondProjectBuild.Branch,
                StringComparison.CurrentCultureIgnoreCase);

            var versionEqual = firstProjectBuild.Version.Equals(secondProjectBuild.Version,
                StringComparison.InvariantCultureIgnoreCase);

            var commitIdEqual = firstProjectBuild.CommitId.Equals(secondProjectBuild.CommitId,
                StringComparison.InvariantCultureIgnoreCase);

            var committerNameEqual = firstProjectBuild.CommitterName.Equals(secondProjectBuild.CommitterName,
                StringComparison.InvariantCultureIgnoreCase);

            var committedDateEqual = firstProjectBuild.Committed.Equals(secondProjectBuild.Committed,
                StringComparison.InvariantCultureIgnoreCase);

            var createdEqual = firstProjectBuild.Created.Equals(secondProjectBuild.Created,
                StringComparison.InvariantCultureIgnoreCase);

            return buildStausEqual && branchEqual && versionEqual && commitIdEqual && committerNameEqual && committedDateEqual
                && createdEqual;
        }

        public int GetHashCode(Project project)
        {
            if (ReferenceEquals(project, null))
                return 0;

            return project.GetHashCode();
        }
    }
}
