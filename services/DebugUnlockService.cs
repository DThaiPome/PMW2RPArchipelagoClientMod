using MelonLoader;
using PMW2RPArchipelagoClientMod.models;
using UnityEngine.InputSystem;

namespace PMW2RPArchipelagoClientMod.services
{
    public class DebugUnlockService : IUnlocksService
    {
        private MelonMod _melonMod;
        public bool FlipKick { get; private set; }
        public bool Dash { get; private set; }
        public bool Bomb { get; private set; }
        public ProgressiveButtBounce ButtBounce { get; private set; }

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
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                ButtBounce = ButtBounce switch
                {
                    ProgressiveButtBounce.None => ProgressiveButtBounce.ButtBounce,
                    ProgressiveButtBounce.ButtBounce => ProgressiveButtBounce.SuperButtBounce,
                    ProgressiveButtBounce.SuperButtBounce => ProgressiveButtBounce.None,
                    _ => ButtBounce
                };
                _melonMod.LoggerInstance.Msg("SWITCHED BUTT BOUNCE UNLOCK TO: " + ButtBounce.ToString());
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                FlipKick = !FlipKick;
                _melonMod.LoggerInstance.Msg("SWITCHED FLIP KICK UNLOCK TO: " +  FlipKick.ToString());
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                Dash = !Dash;
                _melonMod.LoggerInstance.Msg("SWITCHED DASH UNLOCK TO: " + Dash.ToString());
            }
            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                Bomb = !Bomb;
                _melonMod.LoggerInstance.Msg("SWITCHED BOMB UNLOCK TO: " + Bomb.ToString());
            }
        }
    }
}
