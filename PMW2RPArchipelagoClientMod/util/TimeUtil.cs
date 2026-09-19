using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
