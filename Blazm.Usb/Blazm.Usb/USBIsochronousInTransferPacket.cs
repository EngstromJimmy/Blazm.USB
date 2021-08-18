using System.Text.Json.Serialization;

namespace Blazm.Usb
{
    public class USBIsochronousInTransferPacket
    {
        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
        [JsonPropertyName("status")]
        public USBTransferStatus Status { get; set; }
    };
}
