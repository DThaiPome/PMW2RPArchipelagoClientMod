using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.GALAXIAN_OFFSET,
        exclusiveMax: WorldConstants.GALAXIAN_OFFSET + GALAXIAN_CAP)]
    public class GalaxianCollectedLocation : AUnlockableLocation<int>
    {
        public const int GALAXIAN_CAP = 15;

        public static Dictionary<int, GalaxianCollectedLocation> GALAXIAN_LOCATIONS { get; private set; }

        static GalaxianCollectedLocation()
        {
            GALAXIAN_LOCATIONS = new Dictionary<int, GalaxianCollectedLocation>();
            for (var maze = 0; maze < GALAXIAN_CAP; maze++)
            {
                GALAXIAN_LOCATIONS[maze] = new GalaxianCollectedLocation(maze);
            }
        }

        public static bool IsGalaxianCollected(ILocationsSource locations, int maze)
        {
            return maze >= 0 && maze < GALAXIAN_CAP && locations.IsCleared(GALAXIAN_LOCATIONS[maze]);
        }

        public static void ClearGalaxianCollected(ILocationsSource locations, int maze)
        {
            if (maze < 0 || maze >= GALAXIAN_CAP)
            {
                return;
            }
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
