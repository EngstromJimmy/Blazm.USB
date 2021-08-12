using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazm.Usb
{

    [Serializable]
    public class UsbDevice
    {
        private UsbNavigator _usbNavigator;
        public void InitHidDevice(UsbNavigator usbNavigator)
        {
            _usbNavigator = usbNavigator;
        }

        public string Id { get { return $"{VendorId}-{ProductId}"; } }
        public string? ProductName { get; set; }
        public ushort? VendorId { get; set; }
        public ushort? ProductId { get; set; }
        public bool Opened { get; set; }

        public async Task OpenAsync()
        {
            Opened = await _usbNavigator.OpenDeviceAsync(this);
        }

        public async Task CloseAsync()
        {
            Opened = await _usbNavigator.CloseDeviceAsync(this);
        }


        public async Task SendReportAsync(int reportId, byte[] data)
        {
            await _usbNavigator.SendReportAsync(this, reportId, data);
        }

        public async Task SendFeatureReportAsync(int reportId, byte[] data)
        {
            await _usbNavigator.SendFeatureReportAsync(this, reportId, data);
        }

        
        [JSInvokable]
        public void HandleOnInputReport(int reportId,byte[] data)
        {
            
        }

        //attribute EventHandler oninputreport;
        //readonly attribute boolean opened;

       
        //Promise<DataView> receiveFeatureReport([EnforceRange] octet reportId);

    }
}