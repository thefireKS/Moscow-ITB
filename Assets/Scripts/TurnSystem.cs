using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance;
    public Turn currentTurn;
    public int currentTurnNumber = 0;
    public int totalTurns;

    private const int turnsNumber = 3; // 4 in total, but count goes from 0, that's why

    public static Action OnChangingTurn;

    public static Action<GameReplaySystem.result> setTieResult;
    
    public enum Turn
    {
        PlayerMove,
        PlayerAttack,
        EnemyMove,
        EnemyAttack
    }
    private void Awake()
    {
        if (instance == null && instance != this)
            instance = this;
        else
            Destroy(this);
    }

    public void NextTurn()
    {
        currentTurn++;

        if (currentTurn > (Turn) turnsNumber)
        {
            currentTurn = 0;
            currentTurnNumber++;
        }

        if (currentTurnNumber > totalTurns)
        {
            setTieResult(GameReplaySystem.result.Tie);
            return;
        }

        OnChangingTurn?.Invoke();
    }
}