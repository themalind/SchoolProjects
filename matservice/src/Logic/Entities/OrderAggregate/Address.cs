namespace Logic.Entities;

public class Address // ValueObject
{
    public int Id { get; private set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }

#pragma warning disable CS8618
    private Address() { }

    public Address(string street,
        string city,
        string zipcode)
    {
        Street = street;
        City = city;
        ZipCode = zipcode;
    }

}