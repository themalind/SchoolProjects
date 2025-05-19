using Logic.Entities;

namespace Web.Models;
public class OpeningHourViewModel
{
    public required string Day { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public TimeOnly? KitchenCloseTime { get; set; }

    public static OpeningHourViewModel MapToOpeningHoursViewModel(OpeningHours openings)
    {
        return new OpeningHourViewModel
        {
            Day = openings.Day.ToString(),
            OpenTime = openings.OpenTime,
            CloseTime = openings.CloseTime,
            KitchenCloseTime = openings.KitchenCloseTime
        };
    }

}