using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float movementRadius;

    private Camera _camera;

    private List<Vector3> _possiblePositions;

    public static Action<List<Vector3Int>> setMovementTile;
        
    private void Start()
    {
        _camera = Camera.main;
        SetPosition();
    }
    
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            ClickToMove();
    }

    private void ClickToMove()
    {
        Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
        
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;
        
        var hitPosition = raycastHit.transform.position;

        if (!_possiblePositions.Contains(hitPosition)) return;
        
        var newPosition = new Vector3(hitPosition.x, transform.position.y, hitPosition.z);
        transform.position = newPosition;
        GetPossibleTiles();
    }

    private void SetPosition()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;

        var hitPosition = raycastHit.transform.position;
        var newPosition = new Vector3(hitPosition.x, transform.position.y,
            hitPosition.z);

        transform.position = newPosition;
        GetPossibleTiles();
    }

    private void GetPossibleTiles()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, 0, transform.position.z);
        
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, movementRadius);

        List <Vector3Int> _vector3Int = new List<Vector3Int>();
        
        _possiblePositions = new List<Vector3>();

        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("MovementBlock")) continue;
            var hitPosition = hitCollider.transform.position;
            var hitX = hitPosition.x - 0.5f;
            var hitY = hitPosition.z - 0.5f;
            
            
            Debug.Log($"{hitCollider.transform.position.x} vs {hitX} and {hitCollider.transform.position.z} vs {hitY}");
            
            _vector3Int.Add(new Vector3Int((int)hitX,(int)hitY));
            _possiblePositions.Add(hitPosition);
        }
        //Stupidest solution ever made because player has 0.5f offset
        setMovementTile?.Invoke(_vector3Int);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector3 spherePosition = new Vector3(transform.position.x, 0, transform.position.z);
        
        Gizmos.DrawWireSphere(spherePosition, movementRadius);
    }
}
