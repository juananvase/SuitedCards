using System;
using UnityEngine;

[RequireComponent(typeof(TimeManager))]
[RequireComponent(typeof(QTEManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [field: SerializeField] public TimeManager TimeManager { get; private set; }
    [field: SerializeField] public QTEManager QTEManager { get; private set; }

    private void OnValidate()
    {
        QTEManager = GetComponent<QTEManager>();
        TimeManager = GetComponent<TimeManager>();
    }
}
