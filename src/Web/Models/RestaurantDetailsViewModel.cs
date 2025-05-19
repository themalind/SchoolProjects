using Logic.Entities;
using Logic.Enums;

namespace Web.Models;

public class RestaurantDetailsViewModel
{
    public string Name { get; set; } = "";
    public string PictureUri { get; set; } = "";
    public decimal DeliveryFee { get; set; }
    public string Description { get; set; } = "";

    // Extra properties för vyn   
    public List<CourseViewModel> FoodCourses { get; set; } = [];
    public List<OpeningHourViewModel> OpeningHours { get; set; } = [];
    public bool IsOpen { get; set; }
    public bool IsKitchenOpen { get; set; }
    public string StatusMessage { get; set; } = "";

    public static RestaurantDetailsViewModel Create(Restaurant restaurant)
    {
        var today = (WeekDay)DateTime.Today.DayOfWeek;
        var currentTime = TimeOnly.FromDateTime(DateTime.UtcNow.ToLocalTime());
        var openingHours = restaurant.OpeningHours?.FirstOrDefault(o => o.Day == today);

        var viewModel = new RestaurantDetailsViewModel
        {
            Name = restaurant.Name,
            Description = restaurant.Description,
            OpeningHours = restaurant.OpeningHours?.Select(OpeningHourViewModel.MapToOpeningHoursViewModel).ToList() ?? new List<OpeningHourViewModel>(),
            PictureUri = restaurant.PictureUri,
            DeliveryFee = restaurant.DeliveryFee,
            FoodCourses = restaurant.Menu?.Select(CourseViewModel.MapToCourseViewModel).ToList() ?? new List<CourseViewModel>(),
        };

        if (openingHours == null)
        {
            viewModel.IsOpen = false;
            viewModel.StatusMessage = $"{restaurant.Name} är stängt.";
            return viewModel;
        }

        viewModel.IsOpen = currentTime >= openingHours.OpenTime && currentTime <= openingHours.CloseTime;
        viewModel.IsKitchenOpen = viewModel.IsOpen && currentTime <= openingHours.KitchenCloseTime;
        viewModel.StatusMessage = DetermineStatusMessage(viewModel.IsOpen, viewModel.IsKitchenOpen, openingHours);

        return viewModel;
    }

    private static string DetermineStatusMessage(bool isOpen, bool isKitchenOpen,
         OpeningHours openingHours)
    {
        if (!isOpen)
            return "Stängt";

        if (!isKitchenOpen)
            return "Köket är stängt";

        return $"Öppet: {openingHours.OpenTime} - {openingHours.CloseTime}";
    }

}
