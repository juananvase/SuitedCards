using UnityEngine;
public interface IParryable
{
    float ParryEfficiency { get; set; }
    void ParriedAttack(GameObject victim, GameObject instigator);
}
