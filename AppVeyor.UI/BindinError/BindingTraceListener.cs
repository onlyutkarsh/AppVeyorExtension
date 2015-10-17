using System.Diagnostics;
using System.Text;
using System.Windows;

namespace AppVeyor.UI.BindinError
{
    public class BindingErrorTraceListener : DefaultTraceListener
    {
        private static BindingErrorTraceListener _listener;

        public static void SetTrace()
        { SetTrace(SourceLevels.Error, TraceOptions.None); }

        public static void SetTrace(SourceLevels level, TraceOptions options)
        {
            if (_listener == null)
            {
                _listener = new BindingErrorTraceListener();
                PresentationTraceSources.DataBindingSource.Listeners.Add(_listener);
            }

            _listener.TraceOutputOptions = options;
            PresentationTraceSources.DataBindingSource.Switch.Level = level;
        }

        public static void CloseTrace()
        {
            if (_listener == null)
            { return; }

            _listener.Flush();
            _listener.Close();
            PresentationTraceSources.DataBindingSource.Listeners.Remove(_listener);
            _listener = null;
        }



        private StringBuilder _Message = new StringBuilder();

        private BindingErrorTraceListener()
        { }

        public override void Write(string message)
        { _Message.Append(message); }

        public override void WriteLine(string message)
        {
            _Message.Append(message);

            var final = _Message.ToString();
            _Message.Length = 0;

            MessageBox.Show(final, "Binding Error", MessageBoxButton.OK,
              MessageBoxImage.Error);
        }
    }
}
