using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.v2.item.consumables.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.consumables
{
    public class ExtraLifeItem : AUnlockableConsumable<FillerKind>
    {
        private static readonly long FILLER_OFFSET = 10000;
        private static readonly long EXTRA_LIFE_ID = FILLER_OFFSET + (long)FillerKind.ExtraLife;

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterConsumableInRange(EXTRA_LIFE_ID, EXTRA_LIFE_ID + 1, id => new ExtraLifeItem());
        }

        public ExtraLifeItem() : base(EXTRA_LIFE_ID)
        {
        }

        public override FillerKind Consumable => FillerKind.ExtraLife;

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            dispatcher.GiveLives(1);
        }
    }
}
