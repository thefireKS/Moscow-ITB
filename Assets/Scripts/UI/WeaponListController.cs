using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponListController : MonoBehaviour
{
    [SerializeField] private GameObject listOfWeapons;
    [Space(5)]
    [SerializeField] private Transform listArrow;

    private float _cachedRotation = 0;
    
    public void ListToggle()
    {
        _cachedRotation += 180f;
        listArrow.rotation = Quaternion.Euler(0,0,_cachedRotation);
        listOfWeapons.SetActive(!listOfWeapons.activeSelf);
    }
}
