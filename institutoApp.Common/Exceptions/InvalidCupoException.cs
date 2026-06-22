using System;

namespace Academico.Common.Exceptions
{
    public class InvalidCupoException : Exception
    {
        public InvalidCupoException() : base() { }
        public InvalidCupoException(string message) : base(message) { }
        public InvalidCupoException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}