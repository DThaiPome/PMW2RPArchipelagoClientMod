using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables
{
    public class PointsBundle : AUnlockableConsumable<FillerKind>
    {
        private static readonly long FILLER_OFFSET = 10000;
        private static readonly long FILLER_CEIL = FILLER_OFFSET + (long)FillerKind.ExtraLife;

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterConsumableInRange(FILLER_OFFSET + (long)FillerKind.Points100, FILLER_CEIL, id => new PointsBundle(id));
        }

        private FillerKind _kind;

        public PointsBundle(long id) : base(id)
        {
            _kind = (FillerKind)(id - FILLER_OFFSET);
        }

        public PointsBundle(FillerKind kind) : base((long)kind + FILLER_OFFSET)
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
