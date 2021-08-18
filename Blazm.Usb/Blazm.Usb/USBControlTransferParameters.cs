using System.Text.Json.Serialization;

namespace Blazm.Usb
{
    public class USBControlTransferParameters
    {

        [JsonPropertyName("requestType")]
        public USBRequestType RequestType;
        [JsonPropertyName("recipient")]
        public USBRecipient Recipient;
        [JsonPropertyName("request")]
        public byte Request;
        [JsonPropertyName("Value")]
        public short Value;
        [JsonPropertyName("index")]
        public short Index;
    };
}
