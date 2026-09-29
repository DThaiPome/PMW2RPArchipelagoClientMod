using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public interface IUnlockableItem<T> : IUnlockableItemId
    {
        T Item { get; }
    }
}
