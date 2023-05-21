using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableCrowd : MonoBehaviour
{
    public static Action addPeople;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        addPeople?.Invoke();
        Destroy(gameObject);
    }
}
