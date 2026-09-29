using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public class ConsumableDelegates : IConsumableDispatcher
    {
        public Action<int> OnGiveLives { get; set; }
        public Action<int> OnGivePacDots { get; set; }
        public Action<int> OnGivePoints { get; set; }
        public Action OnTriggerVoiceLineTrap { get; set; }

        public void GiveLives(int count)
        {
            OnGiveLives?.Invoke(count);
        }

        public void GivePacDots(int count)
        {
            OnGivePacDots?.Invoke(count);
        }

        public void GivePoints(int count)
        {
            OnGivePoints?.Invoke(count);
        }

        public void TriggerVoiceLineTrap()
        {
            OnTriggerVoiceLineTrap?.Invoke();
        }
    }
}
