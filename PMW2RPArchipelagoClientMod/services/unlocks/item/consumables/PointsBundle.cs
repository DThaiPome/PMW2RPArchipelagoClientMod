using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.FILLER_OFFSET + (long)FillerKind.Points100,
        exclusiveMax: WorldConstants.FILLER_OFFSET + (long)FillerKind.ExtraLife)]
    public class PointsBundle : AUnlockableConsumable<FillerKind>
    {
        private FillerKind _kind;

        public PointsBundle(long id) : base(id)
        {
            _kind = (FillerKind)(id - WorldConstants.FILLER_OFFSET);
        }

        public PointsBundle(FillerKind kind) : base((long)kind + WorldConstants.FILLER_OFFSET)
        {
            _kind = kind;
        }

        public override FillerKind Consumable => _kind;

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            dispatcher.GivePoints(_kind switch
            {
                FillerKind.Points100 => 100,
                FillerKind.Points200 => 200,
                FillerKind.Points500 => 500,
                FillerKind.Points1000 => 1000,
                _ => throw new NotImplementedException()
            });
        }
    }
}
