using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazm.Usb
{
    public class UsbNavigator : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public UsbNavigator(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/Blazm.Usb/Blazm.Usb.js").AsTask());
        }

        public async Task<UsbDevice> RequestDeviceAsync(UsbDeviceRequestOptions options)
        {
            string json = JsonConvert.SerializeObject(options,
            Newtonsoft.Json.Formatting.None,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var module = await moduleTask.Value;
            var device = await module.InvokeAsync<UsbDevice>("requestDevice", json);
            device.InitHidDevice(this);
            return device;
        }


        public async Task<List<UsbDevice>> GetDevicesAsync()
        {
            var module = await moduleTask.Value;
            var devices = await module.InvokeAsync<UsbDevice[]>("getDevices");
            //Init all the devices
            foreach (var d in devices)
            {
                d.InitHidDevice(this);
            }
            return devices.ToList();
        }

        private List<DotNetObjectReference<UsbDevice>> NotificationHandlers = new();

        public async Task<bool> OpenDeviceAsync(UsbDevice device)
        {
            var module = await moduleTask.Value;
            var handler = DotNetObjectReference.Create(device);
            NotificationHandlers.Add(handler);
            return await module.InvokeAsync<bool>("openDevice", device.Id,handler );
        }

        public async Task<bool> CloseDeviceAsync(UsbDevice device)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<bool>("closeDevice", device.Id);
        }

        public async Task SelectConfigurationAsync(UsbDevice device, byte configurationValue)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("selectConfiguration", device.Id, configurationValue);
        }


        public async Task ClaimInterfaceAsync(UsbDevice device, byte interfaceNumber)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("claimInterface", device.Id, interfaceNumber);
        }
        public async Task ReleaseInterface(UsbDevice device, byte interfaceNumber)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("releaseInterface", device.Id, interfaceNumber);
        }
        public async Task SelectAlternateInterfaceAsync(UsbDevice device, byte interfaceNumber,byte alternateSetting)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("selectAlternateInterface", device.Id, interfaceNumber);
        }

        public async Task ResetAsync(UsbDevice device)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("reset", device.Id);
        }

        public async Task<USBInTransferResult> ControlTransferIn(UsbDevice device,USBControlTransferParameters setup, short length)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<USBInTransferResult>("controlTransferIn", device.Id,setup,length);
        }

        
        public async Task<USBInTransferResult> ControlTransferOut(UsbDevice device, USBControlTransferParameters setup, byte[] data)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<USBInTransferResult>("controlTransferOut", device.Id, setup.RequestType.ToString(), setup.Recipient.ToString(), setup.Request, setup.Value, setup.Index,data);
        }

        public async Task ClearHalt(UsbDevice device, USBDirection direction, byte endpointNumber)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("clearHalt", device.Id, direction, endpointNumber);
        }

        public async Task<USBInTransferResult> TransferIn(UsbDevice device,byte endpointNumber, long length)
        {
            var module = await moduleTask.Value;
            var bytes= await module.InvokeAsync<uint[]>("transferIn", device.Id, endpointNumber, length);
            return new USBInTransferResult() { Data=bytes};
        }

        public async Task<USBOutTransferResult> TransferOut(UsbDevice device, byte endpointNumber, byte[] data)
        {
            var module = await moduleTask.Value;
            var value= await module.InvokeAsync<object>("transferOut", device.Id, endpointNumber, data);
            //return await module.InvokeAsync<USBOutTransferResult>("transferOut", device.Id, endpointNumber, data);
            return new USBOutTransferResult();
        }


        public async Task<USBIsochronousInTransferResult> IsochronousTransferIn(UsbDevice device, byte endpointNumber, long[] packetLengths)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<USBIsochronousInTransferResult>("isochronousTransferIn", device.Id, endpointNumber, packetLengths);
        }


        public async Task<USBIsochronousOutTransferResult> IsochronousTransferOut(UsbDevice device, byte endpointNumber, byte[] data, long[] packetLengths)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<USBIsochronousOutTransferResult>("isochronousTransferOut", device.Id, data, packetLengths);
        }
        
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
