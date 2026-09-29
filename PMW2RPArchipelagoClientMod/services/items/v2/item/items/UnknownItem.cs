using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items
{
    public class UnknownItem : AUnlockableItem<long>
    {
        public UnknownItem(long id) : base(id)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Found unknown item with id: " + Id);
        }

        public override long Item => Id;

        public override void Unlock(IItemsDispatcher dispatcher)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Unlocking unknown item with id: " + Id);
        }
    }
}
