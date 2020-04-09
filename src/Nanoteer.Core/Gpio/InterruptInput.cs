using Nanoteer.Core.Exceptions;
using System;
using Windows.Devices.Gpio;

namespace Nanoteer.Core.Gpio
{
    public delegate void InterruptEventHandler(InterruptInput interrupt, bool state);

    public class InterruptInput : IDisposable
    {
        private object objLock = new object();
        private GpioPin _port;
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

            _port = GpioController.GetDefault().OpenPin(cpuPin);
            _port.DebounceTimeout = new TimeSpan(0, 0, 0, 0, 10);
            _port.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _port.ValueChanged += ValueChanged;
        }

        public bool Read()
        {
            GpioPinValue pinValue = _port.Read();
            return pinValue == GpioPinValue.High ? true : false;
        }

        private void ValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            interruptEvent?.Invoke(this, e.Edge == GpioPinEdge.RisingEdge);
        }

        public void Dispose()
        {
            _port.Dispose();
        }
    }
}
