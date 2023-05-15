using Nanoteer.Core.Exceptions;
using System;
using System.Device.Gpio;

namespace Nanoteer.Core.Gpio
{
    public delegate void InterruptEventHandler(InterruptInput interrupt, bool state);

    public class InterruptInput : IDisposable
    {
        private readonly object objLock = new();
        private readonly GpioPin _port;
        private InterruptEventHandler interruptEvent;
        public event InterruptEventHandler Interrupt
        {
            add
            {
                lock (objLock)
                {
                    interruptEvent += value;
                }
            }
            remove
            {
                lock (objLock)
                {
                    interruptEvent -= value;
                }
            }
        }

        public InterruptInput(Socket socket, int cpuPin)
        {
            if (cpuPin == Socket.UnspecifiedPin)
            {
                // this is a mainboard error but should not happen since we check for this, but it doesnt hurt to double-check
                throw new InvalidSocketException(socket, "DigitalOutput");
            }

            GpioController gpioController = new();
            _port = gpioController.OpenPin(cpuPin, PinMode.Input);
            //_port.DebounceTimeout = new TimeSpan(0, 0, 0, 0, 10);
            _port.ValueChanged += ValueChanged;
        }

        public bool Read()
        {
            PinValue pinValue = _port.Read();
            return pinValue == PinValue.High;
        }

        private void ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            interruptEvent?.Invoke(this, e.ChangeType == PinEventTypes.Rising);
        }

        public void Dispose()
        {
            _port.Dispose();
        }
    }
}
