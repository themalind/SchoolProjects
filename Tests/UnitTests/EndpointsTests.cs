using FastEndpoints;

using FluentAssertions;

using FoodAPI.EndPoints;

using Logic.Entities;
using Logic.Interfaces;

using Moq;

namespace UnitTests;

// https://www.codemag.com/Article/2305041/Using-Moq-A-Simple-Guide-to-Mocking-for-.NET

public class EndpointsTests
{
    [Fact]
    public async Task Endpoint_GetRestaurants_Calls_Repo_ListAsync_Once()
    {
        // Arrange
        var mockRepo = new Mock<IRestaurantRepository>();

        IReadOnlyCollection<Restaurant> restaurants = new List<Restaurant> {
            new Restaurant("test1", new List<OpeningHours>(), "dsc", "pic", 100m)};

        mockRepo.Setup(c => c.ListAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(restaurants));

        GetRestaurantsEndpoint getRestaurantsEndpoint = Factory.Create<GetRestaurantsEndpoint>(mockRepo.Object);

        // Act
        await getRestaurantsEndpoint.HandleAsync(CancellationToken.None);

        // Assert
        mockRepo.Verify(repo => repo.ListAsync(It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact]
    public async Task Endpoint_GetRestaurants_ReponseContainsRightProperties()
    {
        // Arrange
        var mockRepo = new Mock<IRestaurantRepository>();

        IReadOnlyCollection<Restaurant> restaurants = new List<Restaurant> {
            new Restaurant("test1", new List<OpeningHours>(), "dsc", "pic", 100m)};

        mockRepo.Setup(c => c.ListAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(restaurants));

        GetRestaurantsEndpoint getRestaurantsEndpoint = Factory.Create<GetRestaurantsEndpoint>(mockRepo.Object);

        // Act
        await getRestaurantsEndpoint.HandleAsync(CancellationToken.None);

        // Assert
        var response = getRestaurantsEndpoint.Response;

        response.Should().NotBeNull();

        response.Restaurants.Should().NotBeNullOrEmpty();
        response.Restaurants.Count.Should().Be(1);

        var restaurant = response.Restaurants.ElementAt(0);
        restaurant.Name.Should().Be("test1");
        restaurant.DeliveryFee.Should().Be(100m);
        restaurant.Description.Should().Be("dsc");
        restaurant.PictureUri.Should().Be("pic");
    }
}

