using System.Reflection;

namespace PMW2RPArchipelagoClientMod.services.util
{
    public class ReflectionService : IReflectionService
    {
        public IEnumerable<Type> GetLoadedTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes();
        }
    }
}
