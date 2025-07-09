using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QTEUIManager : MonoBehaviour
{
    //Data
    [field: SerializeField] public PlayerCharacter PlayerCharacter { get; private set; }
    [SerializeField] private QTEInfoEventAsset OnSequenceGenerated;
    [SerializeField] private QTEInfoEventAsset OnQTEInputRead;
    [SerializeField] private EmptyEventAsset OnQTEEnded;
    
    //Images
    [SerializeField] private Image[] _inputSlots;
    [SerializeField] private Sprite[] _possibleInputs;
    
    //Sequences
    [SerializeField] private Vector2[] _sequence;
    [SerializeField] private Vector2[] _inputSequence;

    private void OnEnable()
    {
        OnSequenceGenerated.AddListener(SpawnSlots);
        OnQTEInputRead.AddListener(CheckQTEInput);
        OnQTEEnded.AddListener(ClearSlots);
    }

    private void OnDisable()
    {
        OnSequenceGenerated.RemoveListener(SpawnSlots);
        OnQTEInputRead.RemoveListener(CheckQTEInput);
        OnQTEEnded.RemoveListener(ClearSlots);
    }
    
    [ContextMenu(nameof(FindSlots))]
    private void FindSlots()
    {
        _inputSlots = GetComponentsInChildren<Image>();
    }
    
    private void SpawnSlots(QTEInfo qteInfo)
    {
        if(qteInfo.Instigator != PlayerCharacter.gameObject) return;
        if( qteInfo.Sequence == null) return;
        
        _sequence = qteInfo.Sequence;

        for (int i = 0; i < _sequence.Length; i++)
        {
            if (_sequence[i] == Vector2.up)
            {
                _inputSlots[i].sprite = _possibleInputs[0];
                _inputSlots[i].color = Color.white;
                continue;
            }
            
            if (_sequence[i] == Vector2.down)
            {
                _inputSlots[i].sprite = _possibleInputs[1];
                _inputSlots[i].color = Color.white;
                continue;
            }
            
            if (_sequence[i] == Vector2.left)
            {
                _inputSlots[i].sprite = _possibleInputs[2];
                _inputSlots[i].color = Color.white;
                continue;
            }
            
            if (_sequence[i] == Vector2.right)
            {
                _inputSlots[i].sprite = _possibleInputs[3];
                _inputSlots[i].color = Color.white;
                continue;
            }
        }
    }
    
    private void CheckQTEInput(QTEInfo qteInfo)
    {
        if(qteInfo.Instigator != PlayerCharacter.gameObject) return;
        if( _sequence == null) return;

        _inputSequence = qteInfo.Sequence;

        for (int i = 0; i <_inputSequence.Length; i++)
        {
            if (_inputSequence[i] == Vector2.zero) continue;
            
            if (_inputSequence[i] == _sequence[i])
            {
                _inputSlots[i].color = Color.green;
                continue;
            }
            
            if (_inputSequence[i] != _sequence[i])
            {
                _inputSlots[i].color = Color.red;
                continue;
            }
            
        }
    }

    private async void ClearSlots()
    {
        await CheckEmptyInputs();

        _sequence = null;
        _inputSequence = null;
        
        for (int i = 0; i < _inputSlots.Length; i++)
        {
            _inputSlots[i].sprite = null;
            _inputSlots[i].color = new Color(255, 255, 255, 0);
        }
    }
    
    private async Task CheckEmptyInputs()
    {
        for (int i = 0; i < _inputSequence.Length; i++)
        {
            if (_inputSequence[i] == Vector2.zero)
            {
                _inputSlots[i].color = Color.red;
                await Awaitable.NextFrameAsync();
            }
        }
        
        await Awaitable.WaitForSecondsAsync(0.3f);
    }

}

public class QTEInfo
{
    public QTEInfo(GameObject instigator, Vector2[] sequence)
    {
        Instigator = instigator;
        Sequence = sequence;
    }
    public GameObject Instigator { get; set; }
    public Vector2[] Sequence { get; set; }
}
