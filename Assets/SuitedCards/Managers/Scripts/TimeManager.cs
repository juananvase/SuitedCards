using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _slowDownFactor;
    
    private bool _isSlowDown = false;

    public async Task DoSlowMotion(float slowDownLength)
    {
        if(_isSlowDown) return;
        _isSlowDown = true;
        await SlowMotionRoutine(slowDownLength);
        _isSlowDown = false;

    }

    private async Task SlowMotionRoutine(float slowDownLength)
    {
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        
        float startTime = Time.unscaledTime;
        float endTime = Time.unscaledTime + slowDownLength;
        
        while (Time.unscaledTime <= endTime)
        {
            Time.timeScale = FloatRemap(startTime, endTime, 0f, 1f, Time.unscaledTime);
            await Awaitable.NextFrameAsync();
        }

        Time.timeScale = 1f;
    }

    private float FloatRemap(float iMin, float iMax, float oMin, float oMax, float value)
    {
        float t = Mathf.InverseLerp(iMin, iMax, value);
        float result = Mathf.Lerp(oMin, oMax, t);
        return result;
    }
}
