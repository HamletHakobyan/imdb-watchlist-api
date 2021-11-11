using System;

namespace Space.ImdbWatchList.Common.Exceptions
{
    public class AlreadyWatchedException : Exception
    {
        public AlreadyWatchedException()
        {
        }

        public AlreadyWatchedException(string message) : base(message)
        {
        }

        public AlreadyWatchedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}