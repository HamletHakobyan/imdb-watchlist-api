using System;

namespace Space.ImdbWatchList.Common.Exceptions
{
    public class NoSuchItemException : Exception
    {
        public NoSuchItemException()
        {
        }

        public NoSuchItemException(string message) : base(message)
        {
        }

        public NoSuchItemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}