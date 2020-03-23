using Nanoteer.Core.Exceptions;
using System;
using Windows.Devices.Adc;

namespace Nanoteer.Core.Gpio
{
    public class AnalogInput : IDisposable
    {
        private AdcChannel _channel;

        public AnalogInput(Socket socket, int analogChannel)
        {
            if (analogChannel == Socket.UnspecifiedPin)
            {
                // this is a mainboard error but should not happen since we check for this, but it doesnt hurt to double-check
                throw new InvalidSocketException(socket, "DigitalOutput");
            }

            _channel = AdcController.GetDefault().OpenChannel(analogChannel);
        }

        public int Read()
        {
            return _channel.ReadValue();
        }

        public double ReadRatio()
        {
            return _channel.ReadRatio();
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
