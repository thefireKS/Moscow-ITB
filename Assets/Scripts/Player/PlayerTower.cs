using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour
{
    public int health = 5;
    [Space(5)]
    [SerializeField] private int peopleCount;
    [SerializeField] private List<TowerBlock> towerBlocks;
    
    
    private int _floorsCount = 1;
    private List<GameObject> gameObjectBlocks;


    private void OnEnable()
    {
        TakeableBuilding.giveBlock += AddTowerBlock;
        TakeableCrowd.addPeople += AddPeople;

        HumanToBlock.addHumanToTowerBlock += InsertPeopleToBlock;

        gameObjectBlocks = new List<GameObject>();
    }

    private void OnDisable()
    {
        TakeableBuilding.giveBlock -= AddTowerBlock;
        TakeableCrowd.addPeople -= AddPeople;
        
        HumanToBlock.addHumanToTowerBlock -= InsertPeopleToBlock;
    }

    private void AddTowerBlock(GameObject newBlock, TowerBlock newTowerBlock)
    {
        var block = Instantiate(newBlock, transform, false);

        var yPos = transform.position.y + _floorsCount;

        block.transform.position = new Vector3(0, yPos, 0);
        block.transform.localPosition = new Vector3(0, block.transform.localPosition.y, 0);
        
        _floorsCount++;
        
        towerBlocks.Add(newTowerBlock);
        gameObjectBlocks.Add(newBlock);
    }

    private void AddPeople()
    {
        peopleCount++;
    }

    private void InsertPeopleToBlock(TowerBlock block)
    {
        towerBlocks.Find(towerBlock => towerBlock.name == block.name).hasHuman = true;
    }

    public void GetDamage(int damageAmount)
    {
        health -= damageAmount;
        _floorsCount--;
        towerBlocks.RemoveAt(towerBlocks.Count);
        Destroy(gameObjectBlocks[gameObjectBlocks.Count]);
        gameObjectBlocks.RemoveAt(gameObjectBlocks.Count);
    }
}
