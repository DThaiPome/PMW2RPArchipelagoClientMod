using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IUnlockableConsumableId
    {
        long Id { get; }

        public void Consume(IConsumableDispatcher dispatcher);
    }
}
