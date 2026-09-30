using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.FILLER_OFFSET,
        exclusiveMax: WorldConstants.FILLER_OFFSET + (long)FillerKind.Points100)]
    public class PacDotBundle : AUnlockableConsumable<FillerKind>
    {
        private FillerKind _kind;

        public PacDotBundle(long id) : base(id)
        {
            _kind = (FillerKind)(id - WorldConstants.FILLER_OFFSET);
        }

        public PacDotBundle(FillerKind fillerKind) : base((long)fillerKind + WorldConstants.FILLER_OFFSET)
        {
            _kind = fillerKind;
        }

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            dispatcher.GivePacDots(_fillerToDotCount(_kind));
        }

        private static int _fillerToDotCount(FillerKind fillerKind)
        {
            return fillerKind switch
            {
                FillerKind.PacDot1 => 1,
                FillerKind.PacDot5 => 5,
                FillerKind.PacDot10 => 10,
                _ => throw new NotImplementedException(),
            };
        }

        public override FillerKind Consumable => _kind;
    }
}
