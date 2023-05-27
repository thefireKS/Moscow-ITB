using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PossibleTileRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap uiTilemap;
    [SerializeField] private Tile possibleMovementTile;
    [SerializeField] private Tile possibleAttackTile;

    private void OnEnable()
    {
        PlayerMovement.setMovementTile += PaintTile;
        PlayerAttack.setAttackTile += PaintTile;
        
        TurnSystem.OnChangingTurn += UpdateTilesOnTurnEnd;
    }

    private void OnDisable()
    {
        PlayerMovement.setMovementTile -= PaintTile;
        PlayerAttack.setAttackTile -= PaintTile;
        
        TurnSystem.OnChangingTurn -= UpdateTilesOnTurnEnd;
    }

    private void PaintTile(List<Vector3Int> position)
    {
        uiTilemap.ClearAllTiles();

        switch (TurnSystem.instance.currentTurn)
        {
            case TurnSystem.Turn.PlayerMove:
            {
                foreach (var tile in position)
                {
                    uiTilemap.SetTile(tile, possibleMovementTile);
                }

                break;
            }
            case TurnSystem.Turn.PlayerAttack:
            {
                foreach (var tile in position)
                {
                    uiTilemap.SetTile(tile, possibleAttackTile);
                }

                break;
            }
        }
    }

    private void UpdateTilesOnTurnEnd()
    {
        uiTilemap.ClearAllTiles();
    }
}
