using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables
{
    [MapFromIdRange(
        inclusiveMin: VOICE_LINE_TRAP_ID,
        exclusiveMax: VOICE_LINE_TRAP_ID + 1)]
    public class VoiceLineTrap : AUnlockableConsumable
    {
        private const long VOICE_LINE_TRAP_ID = 11000;

        public VoiceLineTrap() : base(VOICE_LINE_TRAP_ID)
        {
        }

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            dispatcher.TriggerVoiceLineTrap();
        }
    }
}
