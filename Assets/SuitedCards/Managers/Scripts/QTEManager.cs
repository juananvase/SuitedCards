using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private Vector2[] _randomPatternSequence = null;
    [SerializeField] private Vector2[] _inputPatternSequence = null;
    
    private Vector2[] _possibleInputs = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private int _inputPosition = 0;
    
    public async Task<bool> QTESequence(int inputsNumber, float reactionWindow)
    {
        GenerateQTEInputs(inputsNumber);
        await GameManager.instance.TimeManager.DoSlowMotion(reactionWindow);
        
        bool result = _inputPatternSequence.SequenceEqual(_randomPatternSequence);
        ResetQTEValues();
        
        return result;
    }
    
    private void GenerateQTEInputs(int inputsNumber)
    {
        _inputPatternSequence = new Vector2[inputsNumber];
        _randomPatternSequence = new Vector2[inputsNumber];
        
        for (int i = 0; i < inputsNumber; i++)
        {
            _randomPatternSequence[i] = _possibleInputs[Random.Range(0, _possibleInputs.Length)];
        }
    }

    private void ResetQTEValues()
    {
        _randomPatternSequence = null;
        _inputPatternSequence = null;
        _inputPosition = 0;
    }

    public void ReadQTEInputs(Vector2 value)
    {
        if(_inputPatternSequence == null) return;
        if (_inputPosition > _inputPatternSequence.Length - 1) return;
        
        _inputPatternSequence[_inputPosition] = value;
        _inputPosition++;
    }
}
