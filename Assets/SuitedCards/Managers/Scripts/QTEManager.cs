using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private QTEInfoEventAsset OnSequenceGenerated;
    [SerializeField] private QTEInfoEventAsset OnQTEInputRead;
    [SerializeField] private EmptyEventAsset OnQTEEnded;
    public Vector2[] RandomPatternSequence { get; private set; } = null;
    public Vector2[] InputPatternSequence { get; private set; } = null;
    
    private Vector2[] _possibleInputs = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private int _inputPosition = 0;
    
    public async Task<bool> QTESequence(int inputsNumber, float reactionWindow, GameObject instigator)
    {
        GenerateQTEInputs(inputsNumber);
        QTEInfo qteInfo = new QTEInfo(instigator, RandomPatternSequence);
        OnSequenceGenerated?.Invoke(qteInfo);
        
        await GameManager.instance.TimeManager.DoSlowMotion(reactionWindow);
        
        bool result = InputPatternSequence.SequenceEqual(RandomPatternSequence);
        ResetQTEValues();
        OnQTEEnded?.Invoke();
        
        return result;
    }
    
    private void GenerateQTEInputs(int inputsNumber)
    {
        InputPatternSequence = new Vector2[inputsNumber];
        RandomPatternSequence = new Vector2[inputsNumber];
        
        for (int i = 0; i < inputsNumber; i++)
        {
            RandomPatternSequence[i] = _possibleInputs[Random.Range(0, _possibleInputs.Length)];
        }
    }

    private void ResetQTEValues()
    {
        RandomPatternSequence = null;
        InputPatternSequence = null;
        _inputPosition = 0;
    }

    public void ReadQTEInputs(Vector2 value, GameObject instigator)
    {
        if(InputPatternSequence == null) return;
        if (_inputPosition > InputPatternSequence.Length - 1) return;
        
        InputPatternSequence[_inputPosition] = value;
        QTEInfo qteInfo = new QTEInfo(instigator, InputPatternSequence);
        OnQTEInputRead?.Invoke(qteInfo);
        _inputPosition++;
    }
}

