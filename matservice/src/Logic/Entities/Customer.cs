namespace Logic.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

#pragma warning disable CS8618 
    private Customer() { }
#pragma warning restore CS8618 
    public Customer(string fullName, string phoneNumber, string email)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

}