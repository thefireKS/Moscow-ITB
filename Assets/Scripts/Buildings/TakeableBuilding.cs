using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableBuilding : MonoBehaviour
{
    [SerializeField] private GameObject blockToPass;
    [SerializeField] private TowerBlock block;
    
    private bool _possibleToTake;

    private Outline _outline;

    private Transform _player;

    public static Action <GameObject, TowerBlock> giveBlock;
    public static Action <TowerBlock> giveBlockToInterface;

    private void Start()
    {
        _outline = GetComponent<Outline>();

        _outline.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        _player = other.transform;
        _possibleToTake = true;
        _outline.enabled = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        _possibleToTake = false;
        _outline.enabled = false;
    }

    private void OnMouseDown()
    {
        if(!_possibleToTake || TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerAttack) return;
        _player.LookAt(transform.position);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        giveBlock?.Invoke(blockToPass,block);
        giveBlockToInterface?.Invoke(block);
        TurnSystem.instance.NextTurn();
    }
}
