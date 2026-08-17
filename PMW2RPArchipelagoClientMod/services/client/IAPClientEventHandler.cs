using Archipelago.MultiClient.Net.Models;

namespace PMW2RPArchipelagoClientMod.services.client
{
    public interface IAPClientEventHandler
    {
        void OnConnect();
        void InitItems(IReadOnlyList<ItemInfo> items);
        void InitLocations(IReadOnlyList<long> locationIds);
        void ItemReceived(ItemInfo item);
        void LocationCheckedRemotely(long locationId);
    }
}
