using System;

namespace Academico.Common.Exceptions
{
    public class CourseFullException : Exception
    {
        public CourseFullException() : base() { }
        public CourseFullException(string message) : base(message) { }
        public CourseFullException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}