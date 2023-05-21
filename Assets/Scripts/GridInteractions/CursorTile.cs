using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorTile : MonoBehaviour
{
    [SerializeField] private Tilemap cursorTilemap;
    [SerializeField] private Tile cursorTile;
    
    private Vector3Int _previousMousePos = new Vector3Int();
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
        
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;
        
        var hitPosition = raycastHit.transform.position;

        var hitX = (int) (hitPosition.x - 0.5f);
        var hitY = (int) (hitPosition.z - 0.5f);
        
        var mousePos = new Vector3Int(hitX, hitY, 0);

        if (mousePos.Equals(_previousMousePos)) return;
        
        cursorTilemap.SetTile(_previousMousePos, null);
        cursorTilemap.SetTile(mousePos, cursorTile);
        _previousMousePos = mousePos;
    }
}
