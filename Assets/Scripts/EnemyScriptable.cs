using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptable : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [Space(5)]
    [SerializeField] private EnemyBehaviour[] enemyBehaviours;
    
    private float _current;
    private const float _target = 1, _lerpTolerance = 0.95f;
    
    private Vector3 _currentPosition;
    private Vector3 _newPosition;
    
    [Serializable]
    private class EnemyBehaviour
    {
        public Vector2 moveDirection;
        public Vector2 attackDirection;
        public TowerBlock.BlockType weaponToAttack;
    }

    private void Start()
    {
        _currentPosition = transform.position;
        _newPosition = _currentPosition;
    }

    private void OnEnable() => TurnSystem.OnChangingTurn += ChangeTurn;

    private void OnDisable() => TurnSystem.OnChangingTurn -= ChangeTurn;

    private void Update()
    {
        LerpMoving();
    }

    private void ChangeTurn()
    {
        if(TurnSystem.instance.currentTurn == TurnSystem.Turn.EnemyMove)
            Move();
        if(TurnSystem.instance.currentTurn == TurnSystem.Turn.EnemyAttack)
            Attack();
    }

    private void Move()
    {
        _currentPosition = transform.position;
        _newPosition =
            new Vector3(_currentPosition.x + enemyBehaviours[TurnSystem.instance.currentTurnNumber].moveDirection.x,
                _currentPosition.y,
                _currentPosition.z + enemyBehaviours[TurnSystem.instance.currentTurnNumber].moveDirection.y);
        _current = 0;
        
        TurnSystem.instance.NextTurn();
    }

    private void LerpMoving()
    {
        if(transform.position.x == _newPosition.x && transform.position.z == _newPosition.z) return;

        _current = Mathf.MoveTowards(_current, _target, movementSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(_currentPosition, _newPosition, _current);
        
        if(_current < _lerpTolerance) return;
        
        _current = 1;
    }

    private void Attack()
    {
        Debug.Log("attacking");
        
        TurnSystem.instance.NextTurn();
    }
}