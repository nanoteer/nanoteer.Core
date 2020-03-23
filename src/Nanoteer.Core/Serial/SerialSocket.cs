using Nanoteer.Core.Exceptions;
using System;
using System.Threading;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace Nanoteer.Core.Serial
{
    public delegate void SerialBytesReadEventHandler(SerialSocket serial, byte[] dataRead);

    public class SerialSocket
    {
        private object objLock = new object();
        private SerialDevice _serialDevice;
        private bool watchCharEnabled = false;

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

            _serialDevice = SerialDevice.FromId(socket.SerialPortName);

            _serialDevice.BaudRate = 9600;
            _serialDevice.Parity = SerialParity.None;
            _serialDevice.StopBits = SerialStopBitCount.Two;
            _serialDevice.Handshake = SerialHandshake.None;
            _serialDevice.DataBits = 8;

            if (watchChar != 0)
            {
                _serialDevice.WatchChar = watchChar;
                watchCharEnabled = true;
            }
            _serialDevice.DataReceived += SerialDevice_DataReceived;
        }

        private void SerialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (watchCharEnabled && e.EventType == SerialData.Chars)
            {
                return;
            }
            using (DataReader inputDataReader = new DataReader(_serialDevice.InputStream))
            {
                inputDataReader.InputStreamOptions = InputStreamOptions.Partial;

                // read all available bytes from the Serial Device input stream
                var bytesRead = inputDataReader.Load(_serialDevice.BytesToRead);

                if (bytesRead > 0)
                {
                    byte[] array = new byte[bytesRead];
                    inputDataReader.ReadBytes(array);
                    serialbytesReadEvent?.Invoke(this, array);
                }
            }
        }
    }
}
