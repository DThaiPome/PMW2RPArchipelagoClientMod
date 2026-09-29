using PMW2RPArchipelagoClientMod.services.items.item;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    public class UnknownItem : AUnlockableItem<long>
    {
        public UnknownItem(long id) : base(id)
        {
        }

        public override long Item => Id;

        public override void Unlock(IItemsDispatcher dispatcher)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Unlocking unknown item with id: " + Id);
        }
    }
}
