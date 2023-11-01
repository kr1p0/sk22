namespace SK.Models
{
    public class CentralEuTimeZone
    {
        public static DateTime Get()
        {
            DateTime databaseUtcTime = DateTime.UtcNow;
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            var testDateTime = TimeZoneInfo.ConvertTimeFromUtc(databaseUtcTime, timeZone);
            return testDateTime;
        }
    }
}
