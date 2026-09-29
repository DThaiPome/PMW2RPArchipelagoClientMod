using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IUnlockableConsumable<T> : IUnlockableConsumableId
    {
        T Consumable { get; }
    }
}
