
using System;

namespace Library.Core.Common.Exceptions
{
    public class EmailFormatException : Exception
    {
        public EmailFormatException(string message) : base(message)
        { }
    }
}
