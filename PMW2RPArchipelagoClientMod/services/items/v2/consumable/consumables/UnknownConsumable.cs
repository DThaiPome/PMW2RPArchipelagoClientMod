using PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables
{
    public class UnknownConsumable : AUnlockableConsumable<long>
    {
        public UnknownConsumable(long id) : base(id)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unknown consumable with ID: " + id);
        }

        public override long Consumable => Id;

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Tried to use unknown consumable with ID: " + Id);
        }
    }
}
