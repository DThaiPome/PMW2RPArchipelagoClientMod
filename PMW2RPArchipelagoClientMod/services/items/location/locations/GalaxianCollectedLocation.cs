using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Il2Cpp.StageManager;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    public class GalaxianCollectedLocation : AUnlockableLocation<int>
    {
        private static readonly long GALAXIAN_OFFSET = 4000;
        private static readonly long STAGE_ID_CEIL = GALAXIAN_OFFSET + 15;

        public static Dictionary<int, GalaxianCollectedLocation> GALAXIAN_LOCATIONS { get; private set; }

        static GalaxianCollectedLocation()
        {
            GALAXIAN_LOCATIONS = new Dictionary<int, GalaxianCollectedLocation>();
            for (var maze = 0; maze < 15; maze++)
            {
                GALAXIAN_LOCATIONS[maze] = new GalaxianCollectedLocation(maze);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(GALAXIAN_OFFSET, STAGE_ID_CEIL, id => new GalaxianCollectedLocation(id));
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
            _maze = (int)(id - GALAXIAN_OFFSET);
        }

        public GalaxianCollectedLocation(int maze) : base(maze + GALAXIAN_OFFSET)
        {
            _maze = maze;
        }

        public override int Location => _maze;
    }
}
