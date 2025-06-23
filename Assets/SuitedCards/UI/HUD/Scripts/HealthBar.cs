using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Image _fillBar;

    private void Update()
    {
        if (_health.Equals(null)) return;
        _fillBar.fillAmount = _health.HealthPercentage;
    }
}
