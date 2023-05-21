using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PossibleTileRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap uiTilemap;
    [SerializeField] private Tile possibleMovementTile;

    private void OnEnable() => GridMovement.setMovementTile += PaintTile;

    private void OnDisable() => GridMovement.setMovementTile -= PaintTile;

    private void PaintTile(List<Vector3Int> position)
    {
        uiTilemap.ClearAllTiles();
        foreach (var tile in position)
        {
            uiTilemap.SetTile(tile,possibleMovementTile);   
        }
    }
}
