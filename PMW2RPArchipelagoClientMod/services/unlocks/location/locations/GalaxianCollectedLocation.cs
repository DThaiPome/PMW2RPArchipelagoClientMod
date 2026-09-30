using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.GALAXIAN_OFFSET,
        exclusiveMax: WorldConstants.GALAXIAN_OFFSET + 15)]
    public class GalaxianCollectedLocation : AUnlockableLocation<int>
    {
        public static Dictionary<int, GalaxianCollectedLocation> GALAXIAN_LOCATIONS { get; private set; }

        static GalaxianCollectedLocation()
        {
            GALAXIAN_LOCATIONS = new Dictionary<int, GalaxianCollectedLocation>();
            for (var maze = 0; maze < 15; maze++)
            {
                GALAXIAN_LOCATIONS[maze] = new GalaxianCollectedLocation(maze);
            }
        }

        public static bool IsGalaxianCollected(ILocationsSource locations, int maze)
        {
            return locations.IsCleared(GALAXIAN_LOCATIONS[maze]);
        }

        public static void ClearGalaxianCollected(ILocationsSource locations, int maze)
        {
            locations.Clear(GALAXIAN_LOCATIONS[maze]);
        }

        private int _maze;

        public GalaxianCollectedLocation(long id) : base(id)
        {
            _maze = (int)(id - WorldConstants.GALAXIAN_OFFSET);
        }

        public GalaxianCollectedLocation(int maze) : base(maze + WorldConstants.GALAXIAN_OFFSET)
        {
            _maze = maze;
        }

        public override int Location => _maze;
    }
}
