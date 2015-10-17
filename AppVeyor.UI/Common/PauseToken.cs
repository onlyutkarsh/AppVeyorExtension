using System.Diagnostics;
using System.Threading.Tasks;

namespace AppVeyor.UI.Common
{
    public struct PauseToken
    {
        private readonly PauseTokenSource _source;

        internal PauseToken(PauseTokenSource source)
        {
            _source = source;
        }

        public bool IsPaused
        {
            get { return _source != null && _source.IsPaused; }
        }

        public Task WaitWhilePausedAsync()
        {
            if (IsPaused)
            {
                Debug.WriteLine("PAUSED...");
                return _source.WaitWhilePausedAsync();
            }
            else return PauseTokenSource._completedTask;
        }
    }
}