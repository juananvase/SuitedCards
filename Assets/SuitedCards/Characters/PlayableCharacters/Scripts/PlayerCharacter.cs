using System;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Parry();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryDash();
        }
    }
}
