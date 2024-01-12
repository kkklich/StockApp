using Newtonsoft.Json;

namespace Netsoftware.Xanthos.Common.SecureUrlGenerator;

public abstract class EncodablePayload
{
    public virtual string Serialize()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}