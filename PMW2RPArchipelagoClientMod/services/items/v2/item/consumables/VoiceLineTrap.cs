using PMW2RPArchipelagoClientMod.services.items.v2.item.consumables.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.consumables
{
    public class VoiceLineTrap : AUnlockableConsumable<long>
    {
        private static readonly long VOICE_LINE_TRAP_ID = 11000;

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterConsumableInRange(VOICE_LINE_TRAP_ID, VOICE_LINE_TRAP_ID + 1, id => new VoiceLineTrap());
        }

        public VoiceLineTrap() : base(VOICE_LINE_TRAP_ID)
        {
        }

        public override long Consumable => Id;

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            dispatcher.TriggerVoiceLineTrap();
        }
    }
}
