using Nanoteer.Core.Exceptions;
using System;
using Windows.Devices.Gpio;

namespace Nanoteer.Core.Gpio
{
    public class DigitalOutput : IDisposable
    {
        private GpioPin _port;

        public DigitalOutput(Socket socket, int cpuPin)
        {
            if (cpuPin == Socket.UnspecifiedPin)
            {
                // this is a mainboard error but should not happen since we check for this, but it doesnt hurt to double-check
                throw new InvalidSocketException(socket, "DigitalOutput");
            }

            _port = GpioController.GetDefault().OpenPin(cpuPin);
            _port.SetDriveMode(GpioPinDriveMode.Output);
        }

        public void Write(bool state)
        {
            _port.Write(state ? GpioPinValue.High : GpioPinValue.Low);
        }

        public GpioPinValue Read()
        {
            return _port.Read();
        }

        public void Dispose()
        {
            _port?.Dispose();
        }
    }
}
