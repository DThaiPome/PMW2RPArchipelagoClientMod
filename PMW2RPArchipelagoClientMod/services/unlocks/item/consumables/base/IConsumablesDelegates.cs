namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IConsumablesDelegates
    {
        Action<int> OnGiveLives { get; set; }
        Action<int> OnGivePacDots { get; set; }
        Action<int> OnGivePoints { get; set; }
        Action OnTriggerVoiceLineTrap { get; set; }
    }
}
