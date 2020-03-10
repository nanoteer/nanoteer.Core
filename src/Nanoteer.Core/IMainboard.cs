namespace Nanoteer.Core
{
    public interface IMainboard
    {
        /// <summary>
        /// Allows mainboards to support storage device mounting/umounting.  This provides modules with a list of storage device volume names supported by the mainboard. 
        /// </summary>
        string[] GetStorageDeviceVolumeNames();

        /// <summary>
        /// Functionality provided by mainboard to mount storage devices, given the volume name of the storage device).
        /// This should result in a event if successful.
        /// </summary>
        bool MountStorageDevice(string volumeName);

        /// <summary>
        /// Functionality provided by mainboard to ummount storage devices, given the volume name of the storage device).
        /// This should result in a event if successful.
        /// </summary>
        bool UnmountStorageDevice(string volumeName);

        /// <summary>
        /// When overriden in a derived class, ensures that the pins on R, G and B sockets (which also have other socket types) are available for use for non-display purposes.
        /// If doing this requires rebooting, then the method must reboot and not return.
        /// If there is no onboard display controller, or it is not possible to disable the onboard display controller, then NotSupportedException must be thrown.
        /// </summary>
        void EnsureRgbSocketPinsAvailable();

        /// <summary>
        /// Sets the debug light emiting diode (LED) on or off.  If there is no debug LED, this method returns without setting the out parameter.
        /// </summary>
        /// <param name="on">true if the debug LED should be on.</param>
        void SetDebugLED(bool on);

        /// <summary>
        /// Called after the initialization of the user's program, after the ProgramStarted method and field initializations, but before the Dispatcher is started.
        /// This can be used by the mainboard driver to do tasks that need to occur after modules are initialized.
        /// </summary>
        void PostInit();

        /// <summary>
        /// The name of this mainboard, which is automatically printed to the debug stream at startup.
        /// </summary>
        string MainboardName { get; }

        /// <summary>
        /// The version of this mainboard, which is automatically printed to the debug stream at startup.
        /// </summary>
        string MainboardVersion { get; }
    }
}
