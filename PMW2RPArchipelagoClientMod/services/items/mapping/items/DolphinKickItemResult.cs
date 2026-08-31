using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class DolphinKickItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            if (unlocks.DolphinKick == ProgressiveDolphinKick.SuperDolphinKick)
            {
                return;
            }
            unlocks.DolphinKick++;
        }
    }
}
