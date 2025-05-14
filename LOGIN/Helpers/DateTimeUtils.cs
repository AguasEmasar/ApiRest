namespace LOGIN.Helpers
{
    public class DateTimeUtils
    {

        private static readonly TimeZoneInfo HondurasTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

        public static DateTime DateTimeNowHonduras()
        {
            DateTime utcTime = DateTime.UtcNow;
            DateTime hondurasDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, HondurasTimeZone);
            return hondurasDateTime;
        }

    }
}
