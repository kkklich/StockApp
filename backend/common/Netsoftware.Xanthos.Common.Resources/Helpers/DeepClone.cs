using Newtonsoft.Json;

namespace Netsoftware.Xanthos.Common.Resources.Helpers;

public static class DeepClone
{
    /// <summary>
    ///     Method for cloning properties in class using JsonConvert serializer
    /// </summary>
    /// <returns>
    /// </returns>
    public static T Clone<T>(T data)
    {
        var JSON = JsonConvert.SerializeObject(data);
        return JsonConvert.DeserializeObject<T>(JSON);
    }
}