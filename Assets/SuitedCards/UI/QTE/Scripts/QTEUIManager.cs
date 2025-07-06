using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class QTEUIManager : MonoBehaviour
{
    [SerializeField] private Image[] _inputSlots;
    [SerializeField] private Sprite[] _possibleInputs;

    private void Update()
    {
        SpawnSlots();
    }

    private void SpawnSlots()
    {
        Vector2[] sequence = GameManager.instance.QTEManager.RandomPatternSequence;
        
        if( sequence == null) return;

        for (int i = 0; i < sequence.Length; i++)
        {
            if (sequence[i] == Vector2.up)
            {
                _inputSlots[i].sprite = _possibleInputs[0];
                continue;
            }
            
            if (sequence[i] == Vector2.up)
            {
                _inputSlots[i].sprite = _possibleInputs[1];
                continue;
            }
            
            if (sequence[i] == Vector2.up)
            {
                _inputSlots[i].sprite = _possibleInputs[2];
                continue;
            }
            
            if (sequence[i] == Vector2.up)
            {
                _inputSlots[i].sprite = _possibleInputs[3];
                continue;
            }
        }
    }
}
