using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private int health = 2;
    [SerializeField] private GameObject explosion;
    [Space(5)]
    [SerializeField] private int peopleCount;
    [SerializeField] public List<TowerBlock> towerBlocks;
    
    
    private int _floorsCount = 1;
    private List<GameObject> gameObjectBlocks;
    
    public static Action<GameReplaySystem.result> setLoseResult;
    
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Debug.Log("Got Hit");
            health--;
            var explosionObject = Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
            Destroy(explosionObject,0.5f);
            Destroy(collision.gameObject);
        }

        if (collision.transform.CompareTag("Enemy"))
        {
            health = 0;
        }

        if (health > 0) return;
        
        setLoseResult?.Invoke(GameReplaySystem.result.Lose);
        Destroy(gameObject,0.1f);
    }

    private void OnDestroy()
    {
        for (var i = 0; i < 5; i++)
        {
            var rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
            var instantiate = Instantiate(explosion, rndPosWithin, transform.rotation);
            instantiate.transform.localScale =
                new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
            instantiate.transform.rotation = Quaternion.Euler(Random.Range(0f, 180f),Random.Range(0f, 180f), Random.Range(0f, 180f));
            Destroy(instantiate,0.5f);
        }
    }
}
