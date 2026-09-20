namespace PMW2RPArchipelagoClientMod.util
{
    public class TimeUtil
    {
        public static long NowMs()
        {
            return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        }
    }
}
