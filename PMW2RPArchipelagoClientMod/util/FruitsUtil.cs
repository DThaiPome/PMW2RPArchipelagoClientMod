using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items;
using IUnlocksSource = PMW2RPArchipelagoClientMod.services.items.v2.item.IUnlocksSource;

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
            return FruitSwitchItem.IsFruitSwitchReceived(unlocks, fruit);
        }
    }
}
