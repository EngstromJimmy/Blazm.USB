using System.Text.Json.Serialization;

namespace Blazm.Usb
{
    public class USBOutTransferResult
    {
        [JsonPropertyName("bytesWritten")]
        public long BytesWritten { get; set; }
        [JsonPropertyName("status")]
        public USBTransferStatus Status { get; set; }
    };
}
