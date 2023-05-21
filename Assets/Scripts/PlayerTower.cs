using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private int peopleCount;
    
    private int _floorsCount = 1;
    private GameObject lastBlock;
    private void OnEnable()
    {
        TakeableBuilding.giveBlock += AddTowerBlock;
        TakeableCrowd.addPeople += AddPeople;
    }

    private void OnDisable()
    {
        TakeableBuilding.giveBlock -= AddTowerBlock;
        TakeableCrowd.addPeople -= AddPeople;
    }

    private void AddTowerBlock(GameObject newBlock)
    {
        var block = Instantiate(newBlock, transform, false);

        var yPos = transform.position.y + _floorsCount;

        block.transform.position = new Vector3(0, yPos, 0);
        block.transform.localPosition = new Vector3(0, block.transform.localPosition.y, 0);
        
        _floorsCount++;
        lastBlock = newBlock;
    }

    private void AddPeople()
    {
        peopleCount++;
    }
}
