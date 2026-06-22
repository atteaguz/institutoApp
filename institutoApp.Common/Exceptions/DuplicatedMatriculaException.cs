using System;

namespace Academico.Common.Exceptions
{
    public class DuplicatedMatriculaException : Exception
    {
        public DuplicatedMatriculaException() : base() { }
        public DuplicatedMatriculaException(string message) : base(message) { }
        public DuplicatedMatriculaException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}