using System;

namespace AppVeyor.Api
{
    public class AppVeyorServiceResponse<T>
    {
        public T Result { get; set; }

        public bool HasError { get; set; }

        public Exception Exception { get; set; }
    }
}