using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData/CharacterData")]
public class CharacterData : BaseData
{
    //Attack
    [field: SerializeField] public GameObjectEventAsset OnFindWeapons {get; private set; }
    [field: SerializeField] public float AttackOffset { get; private set; } = 1f;
    [field: SerializeField] public float AttackDuration { get; private set; } = 0.5f;
    
    //Parry
    [field: SerializeField] public GameObjectEventAsset OnStartParryWindow { get; private set; }

    //Dash
    [field: SerializeField] public float DashRate { get; private set; } = 3f;
    [field: SerializeField] public float DashMultiplier { get; private set; } = -7f;
    [field: SerializeField] public float DashYOffset { get; private set; } = 1f;
    [field: SerializeField] public float DashDuration { get; private set; } = 0.5f;
    
}
