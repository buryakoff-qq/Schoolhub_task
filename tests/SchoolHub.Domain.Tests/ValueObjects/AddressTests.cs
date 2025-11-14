using SchoolHub.Domain.ValueObjects;

namespace SchoolHub.Domain.Tests.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Create_WithAllFields_ShouldCreateAddress()
    {
        // Act
        var address = new Address("City", "Street", "12345");

        // Assert
        Assert.Equal("City", address.City);
        Assert.Equal("Street", address.Street);
        Assert.Equal("12345", address.PostalCode);
    }

    [Fact]
    public void Create_WithNullFields_ShouldCreateAddress()
    {
        // Act
        var address = new Address(null, null, null);

        // Assert
        Assert.Null(address.City);
        Assert.Null(address.Street);
        Assert.Null(address.PostalCode);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimValues()
    {
        // Act
        var address = new Address("  City  ", "  Street  ", "  12345  ");

        // Assert
        Assert.Equal("City", address.City);
        Assert.Equal("Street", address.Street);
        Assert.Equal("12345", address.PostalCode);
    }

    [Fact]
    public void Create_WithPartialFields_ShouldCreateAddress()
    {
        // Act
        var address = new Address("City", null, "12345");

        // Assert
        Assert.Equal("City", address.City);
        Assert.Null(address.Street);
        Assert.Equal("12345", address.PostalCode);
    }
}

