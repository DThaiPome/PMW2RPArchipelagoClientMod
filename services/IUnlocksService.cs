using PMW2RPArchipelagoClientMod.models;

namespace PMW2RPArchipelagoClientMod.services
{
    public interface IUnlocksService : IUnlocks
    {
        void OnLateUpdate();
    }
}
