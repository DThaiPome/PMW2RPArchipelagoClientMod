namespace PMW2RPArchipelagoClientMod.services.util
{
    public interface IReflectionService
    {
        IEnumerable<Type> GetLoadedTypes();
    }
}
