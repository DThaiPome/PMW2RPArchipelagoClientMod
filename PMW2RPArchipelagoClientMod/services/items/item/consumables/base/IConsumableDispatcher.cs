using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IConsumableDispatcher
    {
        void GivePacDots(int count);
        void GiveLives(int count);
        void GivePoints(int count);
        void TriggerVoiceLineTrap();
    }
}
