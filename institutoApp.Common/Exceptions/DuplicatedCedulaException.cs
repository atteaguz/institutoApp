using System;

namespace Academico.Common.Exceptions
{
    public class DuplicatedCedulaException : Exception
    {
        public DuplicatedCedulaException() : base() { }
        public DuplicatedCedulaException(string message) : base(message) { }
        public DuplicatedCedulaException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}