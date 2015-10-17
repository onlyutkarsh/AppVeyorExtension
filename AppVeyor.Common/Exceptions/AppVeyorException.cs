using System;
using System.Runtime.Serialization;

namespace AppVeyor.Common.Exceptions
{
    public class AppVeyorException: Exception
    {
        public AppVeyorException()
        { }

        public AppVeyorException(string message)
            : base(message) { }

        public AppVeyorException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public AppVeyorException(string message, Exception innerException)
            : base(message, innerException) { }

        public AppVeyorException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected AppVeyorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
