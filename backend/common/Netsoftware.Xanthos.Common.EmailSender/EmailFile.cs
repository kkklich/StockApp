namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailFile
{
    public EmailFile(string name, byte[] fileData)
    {
        Name = name;
        FileData = fileData;
    }

    public string Name { get; }

    public byte[] FileData { get; }
}