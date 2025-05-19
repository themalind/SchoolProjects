using Logic.Entities;

namespace UnitTests;

public class OrderTests
{
    [Fact]
    public void TotalSum_ShouldReturnSum()
    {
        // Arrange

        decimal deliveryFee = 79;
        decimal serviceFee = ((2 * 149.95m) + (1 * 149.95m) + deliveryFee) * 0.05m;
        decimal expected = Math.Round((2 * 149.95m) + (1 * 149.95m) + deliveryFee + serviceFee, 2);

        var items = new List<OrderItem>{
               new OrderItem("Laxtartar", 2, 149.95m, "REST-0001000", "FOOD-0001000" ),
                new OrderItem("Laxpudding", 1, 149.95m, "REST-0001000", "FOOD-0001005")
            };
        var customer = new Customer("Frodo Baggins", "0707123456", "hejhej@test.com");
        var adress = new Address("Stigen 29", "Borås", "50465");

        var order = new Order(customer, adress, items, deliveryFee, Logic.Enums.OrderStatus.Delivered);

        // Act  

        var sum = order.GetTotalPrice();

        // Assert

        Assert.Equal(expected, sum);
    }

    [Fact]
    public void ChangeStatus_NotValidStatusChange_ShouldReturn_False()
    {
        // Arrange
        decimal deliveryFee = 79;

        var items = new List<OrderItem>{
               new OrderItem("Laxtartar", 2, 149.95m, "REST-0001000", "FOOD-0001000" ),
                new OrderItem("Laxpudding", 1, 149.95m, "REST-0001000", "FOOD-0001005")
        };
        var customer = new Customer("Frodo Baggins", "0707123456", "hejhej@test.com");
        var adress = new Address("Stigen 29", "Borås", "50465");

        var order = new Order(customer, adress, items, deliveryFee, Logic.Enums.OrderStatus.Confirmed);

        // Act
        var isSuccess = order.ChangeStatus(Logic.Enums.OrderStatus.Delivered);

        // Assert
        Assert.False(isSuccess);
    }

    [Fact]
    public void ChangeStatus_ToNextValidStatus_ShouldReturn_True()
    {
        // Arrange
        decimal deliveryFee = 79;

        var items = new List<OrderItem>{
                new OrderItem("Laxtartar", 2, 149.95m, "REST-0001000", "FOOD-0001000" ),
                new OrderItem("Laxpudding", 1, 149.95m, "REST-0001000", "FOOD-0001005")
        };

        var customer = new Customer("Frodo Baggins", "0707123456", "hejhej@test.com");
        var adress = new Address("Stigen 29", "Borås", "50465");

        var order = new Order(customer, adress, items, deliveryFee, Logic.Enums.OrderStatus.Received);

        // Act
        var isSuccess = order.ChangeStatus(Logic.Enums.OrderStatus.Confirmed);

        // Assert
        Assert.True(isSuccess);
    }

    [Fact]
    public void ChangeStatus_ToStatusWithCourierRequired_WithAssignedCourier_ShouldReturn_True()
    {
        // Arrange
        decimal deliveryFee = 79;

        var items = new List<OrderItem>{
                new OrderItem("Laxtartar", 2, 149.95m, "REST-0001000", "FOOD-0001000" ),
                new OrderItem("Laxpudding", 1, 149.95m, "REST-0001000", "FOOD-0001005")
        };

        var customer = new Customer("Frodo Baggins", "0707123456", "hejhej@test.com");
        var adress = new Address("Stigen 29", "Borås", "50465");
        var courier = new Courier("Smeagol");

        var order = new Order(customer, adress, items, deliveryFee, Logic.Enums.OrderStatus.Confirmed);
        order.Courier = courier;

        // Act
        var isSuccess = order.ChangeStatus(Logic.Enums.OrderStatus.CourierAccepted);

        // Assert
        Assert.True(isSuccess);
    }

    [Fact]
    public void ChangeStatus_ToStatusWithCourierRequired_WithoutAssignedCourier_ShouldReturn_False()
    {
        // Arrange
        decimal deliveryFee = 79;

        var items = new List<OrderItem>{
                new OrderItem("Laxtartar", 2, 149.95m, "REST-0001000", "FOOD-0001000" ),
                new OrderItem("Laxpudding", 1, 149.95m, "REST-0001000", "FOOD-0001005")
        };

        var customer = new Customer("Frodo Baggins", "0707123456", "hejhej@test.com");
        var adress = new Address("Stigen 29", "Borås", "50465");

        var order = new Order(customer, adress, items, deliveryFee, Logic.Enums.OrderStatus.Confirmed);

        // Act
        var isSuccess = order.ChangeStatus(Logic.Enums.OrderStatus.CourierAccepted);

        // Assert
        Assert.False(isSuccess);
    }
}
