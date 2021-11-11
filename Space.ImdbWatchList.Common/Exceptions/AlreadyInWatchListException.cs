using System;

namespace Space.ImdbWatchList.Common.Exceptions
{
    public class AlreadyInWatchListException : Exception
    {
        public AlreadyInWatchListException()
        {
        }

        public AlreadyInWatchListException(string message) : base(message)
        {
        }

        public AlreadyInWatchListException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}