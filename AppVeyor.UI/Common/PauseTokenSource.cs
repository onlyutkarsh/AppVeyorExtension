using System.Threading;
using System.Threading.Tasks;

namespace AppVeyor.UI.Common
{
    public class PauseTokenSource
    {
        private volatile TaskCompletionSource<bool> _paused;
        internal static readonly Task _completedTask = Task.FromResult(true);
        public PauseToken Token { get { return new PauseToken(this); } }


        public bool IsPaused
        {
            get { return _paused != null; }
            set
            {
                if (value)
                {
                    //store it in a variable as a reference to a volatile field will not be treated as volatile
                    var paused = _paused;
                    Interlocked.CompareExchange(ref paused, new TaskCompletionSource<bool>(), null);
                }
                else
                {
                    while (true)
                    {
                        var tcs = _paused;
                        if (tcs == null) return;
                        var paused = _paused;
                        if (Interlocked.CompareExchange(ref paused, null, tcs) == tcs)
                        {
                            tcs.SetResult(true);
                            break;
                        }
                    }
                }
            }
        }

        internal Task WaitWhilePausedAsync()
        {
            var cur = _paused;
            return cur != null ? cur.Task : _completedTask;
        }
    }
}
