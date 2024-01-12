using System;
using System.IO;
using ProtoBuf;

namespace Netsoftware.Xanthos.Infrastructure.Services;

public class ProtoService
{
    public string SerializeModelToProto<T>(T model)
    {
        using var memStream = new MemoryStream();
        Serializer.Serialize(memStream, model);
        return Convert.ToBase64String(memStream.ToArray());
    }

    public T DeserializeProtoToModel<T>(string protoString)
    {
        using var stream = new MemoryStream(Convert.FromBase64String(protoString));
        return Serializer.Deserialize<T>(stream);
    }
}