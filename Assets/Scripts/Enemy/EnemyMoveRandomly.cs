using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveRandomly : EnemyBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _lungeForce = 5f;
    [SerializeField]
    private float _lungeDuration = 1f;

    private float _timeToLunge = 0;

    public override void PerformBehaviour()
    {
        _timeToLunge -= Time.deltaTime;

        if (_timeToLunge < 0)
        {
            _timeToLunge = _lungeDuration;
            float random = Random.Range(0f, 2 * Mathf.PI);
            _rb.AddForce(new Vector2(Mathf.Cos(random), Mathf.Sin(random)) * _lungeForce);
        }
    }
}
