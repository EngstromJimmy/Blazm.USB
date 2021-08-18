
namespace Blazm.Usb.Server.Protocols;
public abstract class Protocol
{
    public abstract string GetString(string id, TypeEnum type, MethodEnum method, char data);
    public abstract string DecodeData(string data, string model);

    public long HexToLong(string data)
    {
        return long.Parse(data, System.Globalization.NumberStyles.HexNumber);

    }
}
