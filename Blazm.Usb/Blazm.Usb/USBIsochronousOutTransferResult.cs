using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blazm.Usb
{
    public class USBIsochronousOutTransferResult
    {
        [JsonPropertyName("packets")]
        public USBIsochronousOutTransferPacket[] Packets { get; set; }
    };
}
