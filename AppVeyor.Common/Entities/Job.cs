using System.Reflection;

namespace AppVeyor.Common.Entities
{
    [Obfuscation(Feature = "trigger", Exclude = false)]
    public class Job
    {

        public string JobId { get; set; }

        public string Name { get; set; }

        public bool AllowFailure { get; set; }

        public int MessagesCount { get; set; }

        public int CompilationMessagesCount { get; set; }

        public int CompilationErrorsCount { get; set; }

        public int CompilationWarningsCount { get; set; }

        public int TestsCount { get; set; }

        public int PassedTestsCount { get; set; }

        public int FailedTestsCount { get; set; }

        public int ArtifactsCount { get; set; }

        public string Status { get; set; }

        public string Started { get; set; }

        public string Finished { get; set; }

        public string Created { get; set; }

        public string Updated { get; set; }
    }

}