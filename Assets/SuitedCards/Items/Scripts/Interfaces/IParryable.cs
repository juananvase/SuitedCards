using UnityEngine;
public interface IParryable
{
    float VictimParryEfficiency { get; set; }
    void ParriedAttack(GameObject victim, GameObject instigator, float baseDamage);
}
