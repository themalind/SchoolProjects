using Logic.Enums;

namespace Logic.Entities;
public class OpeningHours
{
    public int Id { get; private set; }
    public WeekDay Day { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public TimeOnly KitchenCloseTime { get; set; }

    private OpeningHours() { }

    public OpeningHours(WeekDay day,
        TimeOnly openTime,
        TimeOnly closeTime,
        TimeOnly kitchenCloseTime)
    {
        Day = day;
        OpenTime = openTime;
        CloseTime = closeTime;
        KitchenCloseTime = kitchenCloseTime;
    }

}