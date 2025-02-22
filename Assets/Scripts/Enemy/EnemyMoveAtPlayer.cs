using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAtPlayer : EnemyMovementBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _lungeForce = 5f;
    [SerializeField]
    private float _lungeDuration = 1f;

    private float _timeToLunge = 0;

    private Vector2 _toPlayer => (_player.transform.position - transform.position).normalized;

    public override void PerformBehaviour()
    {
        _timeToLunge -= Time.deltaTime;

        if (_timeToLunge < 0)
        {
            _timeToLunge = _lungeDuration;

            _rb.AddForce(_toPlayer * _lungeForce);
            BroadcastMove(_toPlayer);
        }
    }
}
