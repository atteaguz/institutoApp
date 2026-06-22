using System;

namespace Academico.Common.Exceptions
{
    public class DuplicatedEmailException : Exception
    {
        public DuplicatedEmailException() : base() { }
        public DuplicatedEmailException(string message) : base(message) { }
        public DuplicatedEmailException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
