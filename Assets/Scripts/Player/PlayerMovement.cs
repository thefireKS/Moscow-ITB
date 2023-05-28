using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movementRadius;
    [SerializeField] private float movementSpeed;
    [Space(5)] 
    [SerializeField] private AnimationCurve curve;

    private float _current;
    private const float _target = 1, _lerpTolerance = 1f;

    private Camera _camera;

    private List<Vector3> _possiblePositions;
    
    private Vector3 _currentPosition;
    private Vector3 _newPosition;

    public static Action<List<Vector3Int>> setMovementTile;

    private void Start()
    {
        _camera = Camera.main;
        SetPosition();
        GetPossibleTiles();
    }

    private void OnEnable() => TurnSystem.OnChangingTurn += GetPossibleTiles;
    
    private void OnDisable() => TurnSystem.OnChangingTurn -= GetPossibleTiles;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            ClickToMove();
        
        LerpMoving();
    }

    private void ClickToMove()
    {
        if(TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerMove || _current is > 0 and < 1) return;

        Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
        
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;
        
        var hitPosition = raycastHit.transform.position;

        if (!_possiblePositions.Contains(hitPosition)) return;

        _currentPosition = transform.position;
        _newPosition = new Vector3(hitPosition.x, _currentPosition.y, hitPosition.z);
        transform.LookAt(_newPosition);
        _current = 0;
    }

    private void LerpMoving()
    {
        if(transform.position.x == _newPosition.x && transform.position.z == _newPosition.z || _current == 1) return;

        _current = Mathf.MoveTowards(_current, _target, movementSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(_currentPosition, _newPosition, curve.Evaluate(_current));
        
        if(_current < _lerpTolerance) return;
        
        _current = 1;
        TurnSystem.instance.NextTurn();
    }

    private void SetPosition()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;

        var hitPosition = raycastHit.transform.position;
        var newPosition = new Vector3(hitPosition.x, transform.position.y,
            hitPosition.z);

        transform.position = newPosition;
        _currentPosition = transform.position;
        _newPosition = _currentPosition;
    }

    private void GetPossibleTiles()
    {
        if(TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerMove) return;

        Vector3 spherePosition = new Vector3(transform.position.x, 0, transform.position.z);
        
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, movementRadius);

        _possiblePositions = new List<Vector3>();

        List<Vector3> _impossiblePositions = (from hitCollider in hitColliders let hitPosition = hitCollider.transform.position where hitCollider.CompareTag("Building") || hitCollider.CompareTag("Takeable") select new Vector3(hitPosition.x, 0, hitPosition.z)).ToList();

        foreach (var hitCollider in hitColliders)
        {
            var hitPosition = hitCollider.transform.position;

            if (_impossiblePositions.Contains(new Vector3(hitPosition.x, 0, hitPosition.z)))
                continue;

            _possiblePositions.Add(hitPosition);
        }

        foreach (var hitCollider in hitColliders)
        {
            var hitPosition = hitCollider.transform.position;

            if (_possiblePositions.Contains(new Vector3(hitPosition.x + 1, hitPosition.y, hitPosition.z)) ||
                _possiblePositions.Contains(new Vector3(hitPosition.x - 1, hitPosition.y, hitPosition.z)) ||
                _possiblePositions.Contains(new Vector3(hitPosition.x, hitPosition.y, hitPosition.z + 1)) ||
                _possiblePositions.Contains(new Vector3(hitPosition.x, hitPosition.y, hitPosition.z - 1))) continue;
            
            _possiblePositions.Remove(hitPosition);
        }

        List <Vector3Int> _vector3Int = (from pos in _possiblePositions let hitX = (int) (pos.x - 0.5f) let hitY = (int) (pos.z - 0.5f) select new Vector3Int(hitX, hitY)).ToList();
        
        setMovementTile?.Invoke(_vector3Int);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector3 spherePosition = new Vector3(transform.position.x, 0, transform.position.z);
        
        Gizmos.DrawWireSphere(spherePosition, movementRadius);
    }
}
