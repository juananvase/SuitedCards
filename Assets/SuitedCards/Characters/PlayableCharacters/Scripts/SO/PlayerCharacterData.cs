using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Scriptable Objects/CharacterData/PlayerCharacterData")]
public class PlayerCharacterData : CharacterData
{
    //QTE
    [field: SerializeField] public float QTEReactionWindow { get; private set; } = 5f;
    
}
