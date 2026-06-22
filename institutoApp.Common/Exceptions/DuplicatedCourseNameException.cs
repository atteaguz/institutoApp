using System;

namespace Academico.Common.Exceptions
{
    public class DuplicatedCourseNameException : Exception
    {
        public DuplicatedCourseNameException() : base() { }
        public DuplicatedCourseNameException(string message) : base(message) { }
        public DuplicatedCourseNameException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}