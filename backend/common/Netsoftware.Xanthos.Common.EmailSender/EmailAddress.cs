namespace Netsoftware.Xanthos.Common.EmailSender;

public class EmailAddress
{
    public EmailAddress()
    {
    }

    public EmailAddress(string name, string address)
    {
        Name = name;
        Address = address;
    }

    public EmailAddress(string firstName, string lastName, string address)
    {
        Name = $"{firstName} {lastName}";
        FirstName = firstName;
        LastName = lastName;
        Address = address;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}