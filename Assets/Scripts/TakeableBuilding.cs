using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableBuilding : MonoBehaviour
{
    [SerializeField] private GameObject blockToPass;
    [SerializeField] private PlayerTower.TowerBlock block;
    
    private bool _possibleToTake;

    public static Action<GameObject, PlayerTower.TowerBlock> giveBlock;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _possibleToTake = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _possibleToTake = false;
    }

    private void OnMouseDown()
    {
        if(!_possibleToTake) return;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        giveBlock?.Invoke(blockToPass,block);
    }
}
