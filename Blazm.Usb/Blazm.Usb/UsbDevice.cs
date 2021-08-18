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

        public async Task ResetAsync()
        {
            await _usbNavigator.ResetAsync(this);
        }

        public async Task ClaimInterfaceAsync(byte interfaceNumber)
        {
            await _usbNavigator.ClaimInterfaceAsync(this, interfaceNumber);
        }

        public async Task SelectConfigurationAsync( byte configurationValue)
        {
            await _usbNavigator.SelectConfigurationAsync(this, configurationValue);
        }

        public async Task SelectAlternateInterfaceAsync( byte interfaceNumber, byte alternateSetting)
        {
            await _usbNavigator.SelectAlternateInterfaceAsync(this, interfaceNumber, alternateSetting);        
        }



        public async Task<USBInTransferResult> ControlTransferIn(USBControlTransferParameters setup, short length)
        {
            return await _usbNavigator.ControlTransferIn(this, setup, length);
        }

        public async Task<USBInTransferResult> ControlTransferOut(USBControlTransferParameters setup, byte[] data)
        {
            return await _usbNavigator.ControlTransferOut(this, setup, data);
        }

        public async Task ClearHalt(USBDirection direction, byte endpointNumber)
        {
            await _usbNavigator.ClearHalt(this, direction, endpointNumber); 
        }

        public async Task<USBInTransferResult> TransferIn(byte endpointNumber, long length)
        {
            return await _usbNavigator.TransferIn(this, endpointNumber, length);   
        }

        public async Task<USBOutTransferResult> TransferOut(byte endpointNumber, byte[] data)
        {
            return await _usbNavigator.TransferOut(this, endpointNumber, data);
        }


        //public async Task<USBIsochronousInTransferResult> IsochronousTransferIn( byte endpointNumber, long[] packetLengths)
        //{
        //    return await _usbNavigator.IsochronousTransferIn(this, endpointNumber, packetLengths); 
        //}


        //public async Task<USBIsochronousOutTransferResult> IsochronousTransferOut( byte endpointNumber, byte[] data, long[] packetLengths)
        //{
        //    return await _usbNavigator.IsochronousTransferOut(this, endpointNumber, data,packetLengths);        
        //}


        [JSInvokable]
        public void HandleOnInputReport(int reportId,byte[] data)
        {
            
        }

        //attribute EventHandler oninputreport;
        //readonly attribute boolean opened;

       
        //Promise<DataView> receiveFeatureReport([EnforceRange] octet reportId);

    }
}