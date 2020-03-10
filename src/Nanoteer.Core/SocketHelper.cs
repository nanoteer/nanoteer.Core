namespace Nanoteer.Core
{
    public static class SocketHelper
    {
        private static void TestPinsPresent(Socket socket, int[] pins, SocketType type)
        {
            for (int i = 0; i < pins.Length; i++)
            {
                if (socket.SocketPins[pins[i]] == Socket.UnspecifiedPin) SocketRegistrationError(socket, "Cpu pin " + pins[i] + " must be specified for socket of type " + type);
            }
        }

        /// <summary>
        /// Registers a socket.  Should be used by mainboards and socket-providing modules during initialization.
        /// </summary>
        /// <param name="socket">The socket to register</param>
        public static void RegisterSocket(Socket socket)
        {
            if (socket.SocketPins == null || socket.SocketPins.Length != 11) SocketRegistrationError(socket, "SocketPins array must be of length 11");
            if ((socket.SupportedTypes & 0) == 0) SocketRegistrationError(socket, "SupportedTypes list is null/empty");
            SocketType type = socket.SupportedTypes;
            int flagMask = 1 << 30; // start with high-order bit...
            while (flagMask > 0)   // loop terminates once all flags have been compared
            {
                switch (type & (SocketType)flagMask)
                {
                    case SocketType.Y:
                        TestPinsPresent(socket, new int[] { 3, 4, 5, 6 }, type);
                        break;
                    default:
                        //SocketRegistrationError(socket, "Socket type '" + type + "' is not supported by Gadgeteer");
                        break;
                }
                flagMask >>= 1;  // bit-shift the flag value one bit to the right
            }

            Socket.TryRegisterSocket(socket);
        }

        private static void SocketRegistrationError(Socket socket, string message)
        {
            //Debug.Print("Warning: socket " + socket + " is not compliant with Gadgeteer : " + message);
        }
    }
}
