using MelonLoader;

namespace PMW2RPArchipelagoClientMod.services
{
    public class PlayerPacmanStateService
    {
        private MelonMod _melonMod;
        private bool _skipEndJump;

        public PlayerPacmanStateService(MelonMod melonMod)
        {
            _melonMod = melonMod;
            _skipEndJump = false;
        }

        public void PushSkipEndJump()
        {
            _skipEndJump = true;
        }

        public bool PopSkipEndJump()
        {
            bool skipEndJump = _skipEndJump;
            _skipEndJump = false;
            return skipEndJump;
        }
    }
}
