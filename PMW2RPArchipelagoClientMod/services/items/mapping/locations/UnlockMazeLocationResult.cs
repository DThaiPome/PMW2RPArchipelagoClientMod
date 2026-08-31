namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class UnlockMazeLocationResult : ILocationMapEntry
    {
        private int _mazeId;

        public UnlockMazeLocationResult(int mazeId)
        {
            _mazeId = mazeId;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            locations.UnlockMaze(_mazeId);
        }
    }
}
