using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using UnityEngine.InputSystem;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public class DebugUnlockService : IDebugUnlocksService
    {
        private MelonMod _melonMod;
        public bool FlipKick { get; set; }
        public bool Dash { get; set; }
        public bool Bomb { get; set; }
        public ProgressiveButtBounce ButtBounce { get; set; }

        public DebugUnlockService(MelonMod melonMod)
        {
            _melonMod = melonMod;
            FlipKick = true;
            Dash = true;
            Bomb = true;
            ButtBounce = ProgressiveButtBounce.SuperButtBounce;
        }

        public void OnLateUpdate()
        {

        }
    }
}
