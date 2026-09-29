using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base
{
    public abstract class AUnlockableLocation<T> : IUnlockableLocation<T>
    {
        protected readonly long _id;
        public long Id => _id;

        public abstract T Location { get; }

        protected AUnlockableLocation(long id)
        {
            _id = id;
        }

        public virtual void Clear(ILocationsDispatcher dispatcher)
        {
            dispatcher.ClearLocation(this);
        }
    }
}
