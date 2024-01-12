using System.IO;
using System.Text.RegularExpressions;
using ProtoBuf;
using ProtoBuf.Meta;

static void CreateProtoFile<T>(string fileName)
{
    var proto = Serializer.GetProto<T>(ProtoSyntax.Proto3);

    var matches = Regex.Matches(proto, @"\w [A-Z]\w* =");
    foreach (Match match in matches)
    {
        var capitalIndex = match.Index + 2;
        proto = proto[..capitalIndex] + proto[capitalIndex].ToString().ToLower() + proto[(capitalIndex + 1)..];
    }

    var mapMatches = Regex.Matches(proto, @"> [A-Z]\w* =");
    foreach (Match match in mapMatches)
    {
        var capitalIndex = match.Index + 2;
        proto = proto[..capitalIndex] + proto[capitalIndex].ToString().ToLower() + proto[(capitalIndex + 1)..];
    }

    File.WriteAllText(fileName, proto);
}