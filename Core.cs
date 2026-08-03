using MelonLoader;

[assembly: MelonInfo(typeof(PMW2RPArchipelagoClientMod.Core), "PMW2RPArchipelagoClientMod", "1.0.0", "DThaiPome", null)]
[assembly: MelonGame("Bandai Namco Entertainment Inc.", "PAC-MAN WORLD 2 Re-PAC")]

namespace PMW2RPArchipelagoClientMod
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}