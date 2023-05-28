using TMPro;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    private int _cachedCurrentTurn = 0;
    private void OnEnable() => TurnSystem.OnChangingTurn += UpdateTurnsLeft;
    
    private void OnDisable() => TurnSystem.OnChangingTurn -= UpdateTurnsLeft;

    private void UpdateTurnsLeft()
    {
        if (TurnSystem.instance.currentTurnNumber <= _cachedCurrentTurn) return;
        
        _cachedCurrentTurn = TurnSystem.instance.currentTurnNumber;
        turnText.text = (TurnSystem.instance.totalTurns - _cachedCurrentTurn).ToString();
    }
}