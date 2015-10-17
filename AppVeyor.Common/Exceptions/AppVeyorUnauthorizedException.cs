using System;
using System.Runtime.Serialization;

namespace AppVeyor.Common.Exceptions
{
    [Serializable]
    public class AppVeyorUnauthorizedException : Exception
    {
        public AppVeyorUnauthorizedException()
        { }

        public AppVeyorUnauthorizedException(string message)
            : base(message) { }

        public AppVeyorUnauthorizedException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public AppVeyorUnauthorizedException(string message, Exception innerException)
            : base(message, innerException) { }

        public AppVeyorUnauthorizedException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected AppVeyorUnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
