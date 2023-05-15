using Nanoteer.Core.Exceptions;
using System.IO.Ports;

namespace Nanoteer.Core.Serial
{
    public delegate void SerialBytesReadEventHandler(SerialSocket serial, byte[] dataRead);

    public class SerialSocket
    {
        private readonly object objLock = new();
        private readonly SerialPort _serialPort;
        private readonly bool watchCharEnabled = false;

        private SerialBytesReadEventHandler serialbytesReadEvent;
        public event SerialBytesReadEventHandler SerialBytesRead
        {
            add
            {
                lock (objLock)
                {
                    serialbytesReadEvent += value;
                }
            }
            remove
            {
                lock (objLock)
                {
                    serialbytesReadEvent += value;
                }
            }
        }


        public SerialSocket(Socket socket, char watchChar)
        {
            if (socket?.SerialPortName == null || socket.SerialPortName.Length == 0)
            {
                // this is a mainboard error but should not happen since we check for this, but it doesnt hurt to double-check
                throw new InvalidSocketException(socket, "SerialSocket");
            }

            _serialPort = new SerialPort(socket.SerialPortName)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.Two,
                Handshake = Handshake.None,
                DataBits = 8
            };

            if (watchChar != 0)
            {
                _serialPort.WatchChar = watchChar;
                watchCharEnabled = true;
            }
            _serialPort.DataReceived += SerialDevice_DataReceived;
        }

        private void SerialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (watchCharEnabled && e.EventType == SerialData.Chars)
            {
                return;
            }

            // read all available bytes from the Serial Device input stream
            var bytesRead = _serialPort.BytesToRead;

            if (bytesRead > 0)
            {
                byte[] array = new byte[bytesRead];
                _serialPort.Read(array, 0, bytesRead);
                serialbytesReadEvent?.Invoke(this, array);
            }
        }
    }
}
