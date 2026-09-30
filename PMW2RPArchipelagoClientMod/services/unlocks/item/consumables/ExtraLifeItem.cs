using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables
{
    [MapFromIdRange(
        inclusiveMin: EXTRA_LIFE_ID,
        exclusiveMax: EXTRA_LIFE_ID + 1)]
    public class ExtraLifeItem : AUnlockableConsumable<FillerKind>
    {
        private const long EXTRA_LIFE_ID = WorldConstants.FILLER_OFFSET + (long)FillerKind.ExtraLife;

        public ExtraLifeItem(long id) : base(id)
        {
        }

        public ExtraLifeItem() : base(WorldConstants.FILLER_OFFSET + (long)FillerKind.ExtraLife)
        {
        }

        public override FillerKind Consumable => FillerKind.ExtraLife;

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            dispatcher.GiveLives(1);
        }
    }
}
