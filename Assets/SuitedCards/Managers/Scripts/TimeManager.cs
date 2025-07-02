using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _slowDownFactor;
    
    private Coroutine _slowDownCoroutine = null;

    public void DoSlowMotion(float slowDownLength)
    {
        if(_slowDownCoroutine != null) return;
        //TODO let the character assign the slowDownLength
        _slowDownCoroutine = StartCoroutine(SlowMotionRoutine(slowDownLength));
    }

    private IEnumerator SlowMotionRoutine(float slowDownLength)
    {
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
        float startTime = Time.unscaledTime;
        float endTime = Time.unscaledTime + slowDownLength;
        
        while (Time.unscaledTime <= endTime)
        {
            yield return Time.timeScale = FloatRemap(startTime, endTime, 0f, 1f, Time.unscaledTime);
        }

        Time.timeScale = 1f;
        _slowDownCoroutine = null;
    }

    private float FloatRemap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float t = Mathf.InverseLerp(iMin, iMax, value);
        float result = Mathf.Lerp(oMin, oMax, t);
        return result;
    }
}
