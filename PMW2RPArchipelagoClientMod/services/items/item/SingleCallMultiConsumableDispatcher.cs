using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public class SingleCallMultiConsumableDispatcher : IConsumableDispatcher
    {
        private List<IConsumableDispatcher> _dispatchers = new List<IConsumableDispatcher>();

        public void AddDispatcher(IConsumableDispatcher dispatcher)
        {
            _dispatchers.Add(dispatcher);
        }

        public void GiveLives(int count)
        {
            foreach (var dispatcher in _dispatchers)
            {
                dispatcher.GiveLives(count);
            }
        }

        public void GivePacDots(int count)
        {
            foreach (var dispatcher in _dispatchers)
            {
                dispatcher.GivePacDots(count);
            }
        }

        public void GivePoints(int count)
        {
            foreach (var dispatcher in _dispatchers)
            {
                dispatcher.GivePoints(count);
            }
        }

        public void TriggerVoiceLineTrap()
        {
            foreach (var dispatcher in _dispatchers)
            {
                dispatcher.TriggerVoiceLineTrap();
            }
        }
    }
}
