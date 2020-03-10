using System;

namespace Nanoteer.Core.Exceptions
{
    public class SocketImmutableAfterRegistrationException : InvalidOperationException
    {
        internal SocketImmutableAfterRegistrationException()
            : base("Socket data is immutable after socket is registered.")
        { }
    }
}
