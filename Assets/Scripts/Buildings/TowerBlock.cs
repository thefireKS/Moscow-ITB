using System;

[Serializable]
public class TowerBlock
{
    public enum BlockType
    {
        Laser,
        Shield,
        Rocket
    }

    public BlockType blockType;
    public bool hasHuman;
}