public interface IHealable
{
    bool IsAlive { get; }
    
    void Heal(HealingInfo healingInfo);
}
