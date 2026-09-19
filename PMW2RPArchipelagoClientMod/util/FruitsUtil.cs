using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.util
{
    public class FruitsUtil
    {
        public static bool MatchItemSwitchUnlocked(EItem item, IUnlocksSource unlocks)
        {
            if (item < EItem.Cherry || item > EItem.Melon)
            {
                return true;
            }
            EFruits fruit = (EFruits)(item - EItem.Cherry);
            return unlocks.FruitSwitches.Contains(fruit);
        }
    }
}
