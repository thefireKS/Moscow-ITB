using System;
using UnityEngine;

[Serializable]
public class TowerBlock
{
    public enum BlockType
    {
        Laser,
        Shield,
        Rocket
    }

    public string name;
    public BlockType blockType;
    public bool hasHuman;
    public Sprite icon;
    public string description;
}