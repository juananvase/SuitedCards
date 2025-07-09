using UnityEngine;

public interface IShieldable
{
    float Shield { get; set; }

    public float Defend(float damageTaken);
}
