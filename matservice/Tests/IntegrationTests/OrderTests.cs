using System.Net.Http.Json;

using FoodAPI.EndPoints;

namespace IntegrationTests;

public class OrderTests : IClassFixture<WebAppFactoryFixture>
{
    private readonly HttpClient _client;
    public OrderTests(WebAppFactoryFixture fixture)
    {
        _client = fixture.Factory.CreateClient();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrdersAsync()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "api/orders");

        request.Headers.Add("x-api-key", "One_Key_To_Rule_Them_All");

        // Act
        var response = await _client.SendAsync(request);

        var content = await response.Content.ReadFromJsonAsync<OrdersResponse>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(content);
        Assert.True(content.Orders.Count > 0);
    }
}
