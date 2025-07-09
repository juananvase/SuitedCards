using UnityEngine;
using UnityEngine.VFX;

namespace SuitedCards.VFX.Scripts
{
    public class BloodVFX : MonoBehaviour
    {
        [field: SerializeField] public VisualEffect BloodVFXGraph { get; private set; }
        [SerializeField] private bool _toggleSpawnContinuously = false;
        [SerializeField] private float _loopDuration = 0.1f;
        [SerializeField] private Vector3 _forceDirection = new Vector3(0f, -9.081f, 0f); // Default gravity
        
        /* 128 is the initialized capacity in VFX graph. Count clamps at capacity limit. Capacity value can be changed in the graph */
        [SerializeField, Range(0,128)] private float _particleCount = 50f;
        [SerializeField] private Vector2 _lifetime = new Vector2(0.5f, 1f); // Duration between every loop. Calculated in seconds
        
        private readonly int _spawnContinuouslyID = Shader.PropertyToID("SpawnContinuously");
        private readonly int _loopDurationID = Shader.PropertyToID("LoopDuration");
        private readonly int _forceDirectionID = Shader.PropertyToID("ForceDirection");
        private readonly int _particleCountID = Shader.PropertyToID("ParticleCount");
        private readonly int _lifetimeID = Shader.PropertyToID("LifetimeRange");
        
        private void OnValidate() 
        {
            if (!BloodVFXGraph)
                BloodVFXGraph = GetComponent<VisualEffect>();
        }

        private void Start()
        {
            BloodVFXGraph.SetBool(_spawnContinuouslyID, _toggleSpawnContinuously);
            BloodVFXGraph.SetFloat(_loopDurationID, _loopDuration);
            BloodVFXGraph.SetVector3(_forceDirectionID, _forceDirection);
            BloodVFXGraph.SetFloat(_particleCountID, _particleCount);
            BloodVFXGraph.SetVector2(_lifetimeID, _lifetime);
        }
    }
}
 