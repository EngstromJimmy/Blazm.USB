using System.Text.Json.Serialization;

namespace Blazm.Usb
{
    public class USBIsochronousInTransferResult
    {
        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
        [JsonPropertyName("packets")]
        public USBIsochronousInTransferPacket[] Packets { get; set; }
    };
}
