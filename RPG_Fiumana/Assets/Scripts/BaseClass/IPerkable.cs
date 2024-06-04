
public interface IPerkable
{
    public void AttachPerk(Perk perkToAttach);
    public void DetachPerk(Perk perkToDetach);
    public float GetStatValue(StatsEngine.RPGStat stat);
}
