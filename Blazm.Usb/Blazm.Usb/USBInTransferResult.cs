using System.Text.Json.Serialization;

namespace Blazm.Usb
{
    public class USBInTransferResult
    {
        [JsonPropertyName("data")]
        public uint[] Data;
        //[JsonPropertyName("status")]
        //public USBTransferStatus Status=USBTransferStatus.Stall;
    };
}
