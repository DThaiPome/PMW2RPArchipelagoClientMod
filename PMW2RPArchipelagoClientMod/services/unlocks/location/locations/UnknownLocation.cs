using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    public class UnknownLocation : AUnlockableLocation<long>
    {
        public override long Location => Id;

        public UnknownLocation(long id) : base(id)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Found unknown location with id: " + Id);
        }

        public override void Clear(ILocationsDispatcher dispatcher)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Cleared unknown location with id: " + Id);
        }
    }
}
