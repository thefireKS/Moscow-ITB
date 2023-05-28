using System;
using UnityEngine;
using UnityEngine.UI;

public class HumanToBlock : MonoBehaviour
{
    [SerializeField] private GameObject blockedButtonIcon;
    [HideInInspector] public TowerBlock towerBlock;
    public static Action<TowerBlock> addHumanToTowerBlock;

    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void AddHumanToBlock()
    {
        blockedButtonIcon.SetActive(true);
        addHumanToTowerBlock?.Invoke(towerBlock);
        Destroy(_button);
    }
}
