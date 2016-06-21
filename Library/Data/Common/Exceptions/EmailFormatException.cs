
using System;

namespace Library.Data.Common.Exceptions
{
    public class EmailFormatException : Exception
    {
        public EmailFormatException(string message) : base(message)
        { }
    }
}
