using System;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    [SerializeField] private LayerMask _targetMask;
    private void Update()
    {
        SelectTarget();
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryDash();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Parry();
        }
    }

    private void SelectTarget()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _targetMask))
        {
            _target = hitInfo.collider.gameObject;
        }

    }
}
