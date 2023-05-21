using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takeable : MonoBehaviour
{
    [SerializeField] private GameObject blockToPass;
    
    private bool _possibleToTake;

    public static Action<GameObject> giveBlock;
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
        giveBlock?.Invoke(blockToPass);
    }
}
