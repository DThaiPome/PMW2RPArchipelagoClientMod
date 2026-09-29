using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables
{
    public class PacDotBundle : AUnlockableConsumable<FillerKind>
    {
        private static readonly long FILLER_OFFSET = 10000;
        private static readonly long FILLER_CEIL = (long)FillerKind.ExtraLife;

        static PacDotBundle()
        {
            ServiceFactory.IdMapperService.RegisterConsumableInRange(FILLER_OFFSET, FILLER_CEIL, id => new PacDotBundle(id));
        }

        private FillerKind _kind;

        public PacDotBundle(long id) : base(id)
        {
            _kind = (FillerKind)(id - FILLER_OFFSET);
        }

        public PacDotBundle(FillerKind fillerKind) : base((long)fillerKind + FILLER_OFFSET)
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
                FillerKind.PacDot100 => 100,
                FillerKind.PacDot200 => 200,
                FillerKind.PacDot500 => 500,
                FillerKind.PacDot1000 => 1000,
                _ => throw new NotImplementedException(),
            };
        }

        public override FillerKind Consumable => _kind;
    }
}
