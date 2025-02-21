using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private float _detectionRange = 5f;

    [SerializeField]
    private EnemyBehaviour _passiveBehaviour;

    [SerializeField]
    private EnemyBehaviour _aggressiveBehaviour;

    private float _distToPlayer => Vector2.Distance(transform.position, _player.transform.position);

    private void OnEnable()
    {
        _passiveBehaviour.Initialize(_player);
        _aggressiveBehaviour.Initialize(_player);
    }

    private void Update()
    {
        if(_distToPlayer < _detectionRange)
        {
            _aggressiveBehaviour.PerformBehaviour();
        }
        else
        {
            _passiveBehaviour.PerformBehaviour();
        }
    }
}
