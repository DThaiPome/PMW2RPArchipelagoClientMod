using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.consumables.@base
{
    public interface IUnlockableConsumable<T> : IUnlockableConsumableId
    {
        T Consumable { get; }
    }
}
