namespace ASPAPI.Service;

public class MainService
{
    public static DateOnly getDateNow()
    {
        DateTime currentDateTime = DateTime.Now;
        return new DateOnly(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);
    }
}