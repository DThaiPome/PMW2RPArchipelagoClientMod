namespace PMW2RPArchipelagoClientMod.services.unlocks.registry
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MapFromIdRange : Attribute
    {
        public long InclusiveMin { get; private set; }
        public long ExclusiveMax { get; private set; }

        public MapFromIdRange(long inclusiveMin = 0, long exclusiveMax = long.MaxValue)
        {
            InclusiveMin = inclusiveMin;
            ExclusiveMax = exclusiveMax;
        }
    }
}
