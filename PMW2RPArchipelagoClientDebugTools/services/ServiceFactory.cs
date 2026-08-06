using MelonLoader;
using PMW2RPArchipelagoClientDebugTools.services.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientDebugTools.services
{
    public class ServiceFactory
    {
        private static MelonPlugin _melonPlugin = null;
        private static DebugUIService _debugUIService = null;

        public static void Init(MelonPlugin melonPlugin)
        {
            if (melonPlugin == null)
            {
                throw new ArgumentNullException("MELON MOD NULL");
            }
            _melonPlugin = melonPlugin;
        }

        public static MelonPlugin GetModInstance()
        {
            if (_melonPlugin == null)
            {
                throw new InvalidDataException("MELON MOD NULL");
            }
            return _melonPlugin;
        }

        public static DebugUIService GetDebugUIService()
        {
            if (_debugUIService == null)
            {
                if (_melonPlugin == null)
                {
                    throw new InvalidDataException("MELON MOD NULL");
                }
                _debugUIService = new DebugUIService(_melonPlugin);
            }
            return _debugUIService;
        }
    }
}
