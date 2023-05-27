using System;
using System.Linq;
using UnityEngine;

public class TakeableCrowd : MonoBehaviour
{
    public static Action addPeople;

    private Outline _outline;
    private float _playerMovementSphere;
    
    private void Start()
    { 
        _outline = GetComponent<Outline>();
        _playerMovementSphere = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().movementRadius;

        _outline.enabled = false;
    }

    private void OnEnable() => TurnSystem.OnChangingTurn += HighlightForPlayerTaking;

    private void OnDisable() => TurnSystem.OnChangingTurn -= HighlightForPlayerTaking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            addPeople?.Invoke();
            Destroy(gameObject);
        }
        if(other.CompareTag("Enemy"))
            Destroy(gameObject);
    }

    private void HighlightForPlayerTaking()
    {
        if (TurnSystem.instance.currentTurn != TurnSystem.Turn.PlayerMove)
        {
            _outline.enabled = false;
            return;
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _playerMovementSphere);

        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Player"))
                _outline.enabled = true;
        }
    }
}
