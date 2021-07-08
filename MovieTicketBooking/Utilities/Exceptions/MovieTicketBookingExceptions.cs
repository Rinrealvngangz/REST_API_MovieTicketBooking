using System;

namespace Utilities.Exceptions
{
    public class MovieTicketBookingExceptions : Exception
    {
        public MovieTicketBookingExceptions(string message) : base(message)
        {
        }

        public MovieTicketBookingExceptions() : base()
        {
        }

        public MovieTicketBookingExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}