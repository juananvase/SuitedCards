using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthBase healthBase;
    [SerializeField] private Image _fillBar;

    private void Update()
    {
        if (healthBase.Equals(null)) return;
        _fillBar.fillAmount = healthBase.HealthPercentage;
    }
}
