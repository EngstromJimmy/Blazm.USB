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


    var devices = await navigator.usb.requestDevice(objfilter);
    console.log(devices);
    var device = devices[0];
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

    //Add NotificationHandler
    device.NotificationHandler = devicehandler;
    device.oninputreport = async (e) => {
        console.log(e.data)
        await e.srcElement.NotificationHandler.invokeMethodAsync('HandleOnInputReport', e.reportId, new Uint8Array(e.data.buffer));
    };

    await device.open();
    return device.opened;
}



export async function selectConfiguration(deviceId, configuration) {
    var device = getDevice(deviceId);

    await device.configuration(configuration);

}


export async function claimInterface(deviceId, interfaceId) {
    var device = getDevice(deviceId);

    await device.claimInterface(interfaceId);

}

export async function controlTransferOut(deviceId, requestType, recipient, request, value, index) {
    var device = getDevice(deviceId);

    await device.controlTransferOut({
        requestType: requestType,
        recipient: recipient,
        request: request,
        value: value,
        index: index
    });
}


export async function transferIn(deviceId, requestType, recipient, request, value, index) {
    
    var device = getDevice(deviceId);
    await device.controlTransferOut({
        requestType: requestType,
        recipient: recipient,
        request: request,
        value: value,
        index: index
    });
}

function getDevice(deviceId) {
    var paireddevices = PairedUSBDevices.filter(function (device) {
        return device.vendorId + '-' + device.productId == deviceId;
    });

    var device = paireddevices[0];
    return paireddevices;
}