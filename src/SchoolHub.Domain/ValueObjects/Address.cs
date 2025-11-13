namespace SchoolHub.Domain.ValueObjects;

public sealed class Address
{
    public string? City { get; }
    public string? Street { get; }
    public string? PostalCode { get; }
    
    private Address() { }

    public Address(string city, string street, string postalCode)
    {
        City = city?.Trim();
        Street = street?.Trim();
        PostalCode = postalCode?.Trim();
    }
}