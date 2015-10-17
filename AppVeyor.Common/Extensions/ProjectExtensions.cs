using System;
using System.Linq;
using AppVeyor.Common.Entities;

namespace AppVeyor.Common.Extensions
{
    public static class ProjectExtensions
    {
        public static bool IsInProgress(this Project project)
        {
            var builds = project.Builds.FirstOrDefault();

            if (builds != null && builds.Status.Any())
            {
                var status = builds.Status.First();

                return (status.EqualsToStatus(BuildStatus.Running)
                        || status.EqualsToStatus(BuildStatus.Queued));
            }
            return false;
        }
        public static bool EqualsToStatus(this string statusString, BuildStatus buildStatus)
        {
            BuildStatus status;
            return Enum.TryParse(statusString, true, out status) && status == buildStatus;
        }

        public static string ToBuildStatusString(this string statusString)
        {
            BuildStatus status;
            return Enum.TryParse(statusString, true, out status) ? status.ToString() : BuildStatus.Unknown.ToString();
        }

        public static string ToBuildStatusString(this Project project)
        {
            var builds = project.Builds.FirstOrDefault();

            if (builds != null && builds.Status.Any())
            {
                var status = builds.Status.First();
                return status.ToBuildStatusString();
            }
            return "Unknown";
        }

        public static string ToLastRunString(this Project project, bool includeBuildDuration = false)
        {
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                var status = build.Status.First();
                DateTime started;
                DateTime finished;
                DateTime.TryParse(build.Started, out started);
                DateTime.TryParse(build.Finished, out finished);
                BuildStatus buildStatus;
                Enum.TryParse(status, true, out buildStatus);
                switch (buildStatus)
                {
                    case BuildStatus.Failed:
                    case BuildStatus.Success:
                        {
                            if ((DateTime.TryParse(build.Started, out started) && started != DateTime.MinValue) &&
                                (DateTime.TryParse(build.Finished, out finished) && finished != DateTime.MinValue) &&
                                started < finished)
                            {
                                var runDuration = finished.Subtract(started);
                                var lastRunStringWithBuildDuration = string.Format("{0} in {1} min {2} sec", started.PrettyDate(), runDuration.Minutes, runDuration.Seconds);
                                var lastRunString = started.PrettyDate();

                                return includeBuildDuration ? lastRunStringWithBuildDuration : lastRunString;
                            }
                            return started.PrettyDate();
                        }
                    case BuildStatus.Cancelled:
                        {
                            return string.Format("Cancelled {0}", finished.PrettyDate());
                        }
                    case BuildStatus.Queued:
                        {
                            return string.Format("Queued {0}", build.Created.GetHumanizedRunningFromTime());
                        }
                    case BuildStatus.Running:
                        {
                            return string.Format("Running {0}", build.Started.GetHumanizedRunningFromTime());
                        }
                }

                return "???";
            }
            return string.Empty;
        }

        public static string ToBuildVerionString(this Project project)
        {
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                return build.Version;
            }
            return null;
        }

        public static string ToCommitIdString(this Project project)
        {
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                return build.CommitId;
            }
            return null;
        }
    }
}
