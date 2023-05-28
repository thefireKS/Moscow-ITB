using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingRandomiser : MonoBehaviour
{
    [SerializeField] private GameObject[] buildings;

    private void Start()
    {
        Instantiate(buildings[Random.Range(0, buildings.Length)], transform.position, Quaternion.identity);
    }
}