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

        public async Task SendReportAsync(UsbDevice device, int reportId, byte[] data)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("sendReport", device.Id, reportId, data);
        }

        public async Task SendFeatureReportAsync(UsbDevice device, int reportId, byte[] data)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("sendFeatureReport", device.Id, data);
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
