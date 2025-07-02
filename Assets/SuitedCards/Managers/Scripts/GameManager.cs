using System;
using UnityEngine;

[RequireComponent(typeof(TimeManager))]
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

    private void OnValidate()
    {
        TimeManager = GetComponent<TimeManager>();
    }
}
