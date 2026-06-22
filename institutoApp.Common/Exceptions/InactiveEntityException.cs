using System;

namespace Academico.Common.Exceptions
{
    public class InactiveEntityException : Exception
    {
        public InactiveEntityException() : base() { }
        public InactiveEntityException(string message) : base(message) { }
        public InactiveEntityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}