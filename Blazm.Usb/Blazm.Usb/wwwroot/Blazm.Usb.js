var PairedUSBDevices = [];


export async function hookUpEvents() {
    navigator.usb.addEventListener('connect', ({ device }) => {
        console.log(`USB connected: ${device.productName}`);
    });

    navigator.usb.addEventListener('disconnect', ({ device }) => {
        console.log(`USB disconnected: ${device.productName}`);
    });
}

export async function requestDevice(filter)
{
    var objfilter = JSON.parse(filter);
    console.log(filter);


    var device = await navigator.usb.requestDevice(objfilter);
    console.log(device);
    PairedUSBDevices.push(device);

    return returnUsbDevice(device);
}

function returnUsbDevice(device) {
    return {
        "ProductName": device.productName,
        "Id": device.vendorId + '-' + device.productId,
        "VendorId": device.vendorId,
        "ProductId":device.productId
    };
}

export async function closeDevice(deviceId) {
    var device = devices.filter(function (item) {
        return item.id == deviceId;
    });
    device[0].close();
}



export async function getDevices() {
    PairedUSBDevices = await navigator.usb.getDevices();
    console.log(PairedUSBDevices);
    return PairedUSBDevices.map(returnUsbDevice);
}

export async function openDevice(deviceId,devicehandler) {
    var device = getDevice(deviceId);

    ////Add NotificationHandler
    //device.NotificationHandler = devicehandler;
    //device.oninputreport = async (e) => {
    //    console.log(e.data)
    //    await e.srcElement.NotificationHandler.invokeMethodAsync('HandleOnInputReport', e.reportId, new Uint8Array(e.data.buffer));
    //};
    console.log("Go device");
    console.log(device);
    await device.open();
    return device.opened;
}



export async function selectConfiguration(deviceId, configuration) {
    var device = getDevice(deviceId);

    await device.selectConfiguration(configuration);

}


export async function claimInterface(deviceId, interfaceId) {
    var device = getDevice(deviceId);

    await device.claimInterface(interfaceId);

}


export async function reset(deviceId, interfaceId) {
    var device = getDevice(deviceId);

    await device.reset();

}
export async function controlTransferOut(deviceId, requestType, recipient, request, value, index,data) {
    var device = getDevice(deviceId);
    //await device.controlTransferOut({
    //    requestType: requestType,
    //    recipient: recipient,
    //    request: request,
    //    value: value,
    //    index: index
    //});

    await device.controlTransferOut({
        requestType: 'vendor',
        recipient: 'endpoint',
        request: 0,
        value: 0,
        index: 2
    });
}

export async function transferIn(deviceId,endpointNumber, length)
{
    var device = getDevice(deviceId);
    var value = await device.transferIn(endpointNumber, length);
    var bytes = Array.from(new Uint8Array(value.data.buffer));

    //var returnvalue = { status: value.status.capitalize(), data: b2 };
    return bytes;
}

export async function transferOut(deviceId,endpointNumber, data)
{
    var device = getDevice(deviceId);
    var value = await device.transferOut(endpointNumber, data);
    return { bytesWritten: value.bytesWritten, status: value.status };
}
//Promise < USBIsochronousInTransferResult > isochronousTransferIn(octet endpointNumber, sequence < unsigned long > packetLengths);
//Promise < USBIsochronousOutTransferResult > isochronousTransferOut(octet endpointNumber, BufferSource data, sequence < unsigned long > packetLengths);



function getDevice(deviceId) {
    var paireddevices = PairedUSBDevices.filter(function (device) {
        return device.vendorId + '-' + device.productId == deviceId;
    });

    var device = paireddevices[0];
    return device;
}

String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}