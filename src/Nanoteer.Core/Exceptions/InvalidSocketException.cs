using System;

namespace Nanoteer.Core.Exceptions
{
    /// <summary>
    /// An exception raised when an invalid socket is specified, e.g. a socket incompatible with the functionality required. 
    /// </summary>
    public class InvalidSocketException : ArgumentException
    {
        /// <summary>
        /// Generates a new invalid socket exception
        /// </summary>
        /// <param name="message">The exception cause</param>
        public InvalidSocketException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Generates a new invalid socket exception
        /// </summary>
        /// <param name="message">The exception cause</param>
        /// <param name="e">The underlying exception</param>
        public InvalidSocketException(string message, Exception e)
            : base(message, e)
        {
        }

        /// <summary>
        /// Returns an <see cref="InvalidSocketException" /> with functionality error message.
        /// </summary>
        /// <param name="socket">The socket that has the error.</param>
        /// <param name="iface">The interface that is causing the error.</param>
        /// <returns>An <see cref="InvalidSocketException" /> with functionality error message.</returns>
        /// <remarks>
        /// This method helps lowering the footprint and should be called when implementing a socket interface.
        /// </remarks>
        public InvalidSocketException(Socket socket, string iface)
            : base(DirtyMessageHack(socket, iface))
        {
        }

        /// <summary>
        /// Throws an <see cref="InvalidSocketException" /> if a pin number is not in the specified range.
        /// </summary>
        /// <param name="pin">The pin number to test.</param>
        /// <param name="from">The lowest valid pin number.</param>
        /// <param name="to">The highest valid pin number.</param>
        /// <param name="iface">The requesting interface.</param>
        /// <param name="module">The requesting module.</param>
        /// <exception cref="InvalidSocketException">The <paramref name="pin" /> is out of the range specified by <paramref name="from" /> and <paramref name="to" />.</exception>
        /// <remarks>
        /// This method helps lowering the footprint and should be called when implementing a socket interface.
        /// </remarks>
        public static void ThrowIfOutOfRange(SocketPin pin, SocketPin from, SocketPin to, string iface)
        {
            if (pin >= from && pin <= to)
                return;

            string message = "Cannot use " + iface + " interface on pin " + pin + " - pin must be in range " + from + " to " + to + ".";

            throw new InvalidSocketException(message);
        }

        private static string DirtyMessageHack(Socket socket, string iface)
        {
            return "Socket " + socket + " has an error with its " + iface + " functionality. Please try a different socket.";
        }
    }
}
