using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private Transform attackPoint;
    [Space(5)]
    [SerializeField] private float laserAttackRadius;
    
    private Camera _camera;
    private GameObject _currentBullet;
    private bool _bulletExists;
    
    private List<Vector3> _possiblePositions;

    public static Action<List<Vector3Int>> setAttackTile;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable() => TurnSystem.OnChangingTurn += GetPossibleTiles;
    
    private void OnDisable() => TurnSystem.OnChangingTurn -= GetPossibleTiles;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ClickToAttack();

        BulletChecking();
    }

    private void ClickToAttack()
    {
        if(TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerAttack) return;

        Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
        
        if (!Physics.Raycast(ray, out RaycastHit raycastHit)) return;
        
        var hitPosition = raycastHit.transform.position;

        if (!_possiblePositions.Contains(hitPosition)) return;
        
        var _newPosition = new Vector3(hitPosition.x, transform.position.y, hitPosition.z);
        transform.LookAt(_newPosition);

        _currentBullet = Instantiate(laser);
        _currentBullet.transform.position = attackPoint.position;
        _currentBullet.transform.rotation = attackPoint.rotation;

        _bulletExists = true;
    }

    private void BulletChecking()
    {
        if(TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerAttack) return;

        if (_currentBullet == null && _bulletExists)
        {
            TurnSystem.instance.NextTurn();
            _bulletExists = false;
        }
    }

    private void GetPossibleTiles()
    {
        if(TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerAttack) return;
        
        Vector3 spherePosition = new Vector3(transform.position.x, 0, transform.position.z);
        
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, laserAttackRadius);

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

        setAttackTile?.Invoke(_vector3Int);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        Vector3 spherePosition = new Vector3(transform.position.x, 0, transform.position.z);
        
        Gizmos.DrawWireSphere(spherePosition, laserAttackRadius);
    }
}
