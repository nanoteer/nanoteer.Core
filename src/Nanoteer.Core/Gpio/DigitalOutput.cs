using Nanoteer.Core.Exceptions;
using System;
using System.Device.Gpio;

namespace Nanoteer.Core.Gpio
{
    public class DigitalOutput : IDisposable
    {
        private readonly GpioPin _port;

        public DigitalOutput(Socket socket, int cpuPin)
        {
            if (cpuPin == Socket.UnspecifiedPin)
            {
                // this is a mainboard error but should not happen since we check for this, but it doesnt hurt to double-check
                throw new InvalidSocketException(socket, "DigitalOutput");
            }

            GpioController gpioController = new();
            _port = gpioController.OpenPin(cpuPin, PinMode.Output);
        }

        public void Write(bool state)
        {
            _port.Write(state ? PinValue.High : PinValue.Low);
        }

        public PinValue Read()
        {
            return _port.Read();
        }

        public void Dispose()
        {
            _port?.Dispose();
        }
    }
}
