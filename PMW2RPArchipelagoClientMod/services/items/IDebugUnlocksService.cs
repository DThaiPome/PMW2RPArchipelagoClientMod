using PMW2RPArchipelagoClientMod.models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public interface IDebugUnlocksService : IUnlocksService
    {
        public new bool FlipKick { get; set; }
        public new bool Dash { get; set; }
        public new bool Bomb { get; set; }
        public new ProgressiveButtBounce ButtBounce { get; set; }
    }
}
