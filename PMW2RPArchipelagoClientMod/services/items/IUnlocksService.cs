using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public interface IUnlocksService : IUnlocksSource
    {
        void OnLateUpdate();
    }
}
