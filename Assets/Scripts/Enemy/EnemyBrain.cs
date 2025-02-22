using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private SpriteRenderer _spr;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private float _detectionRange = 5f;

    [SerializeField]
    private EnemyMovementBehaviour _passiveMoveBehaviour;

    [SerializeField]
    private EnemyMovementBehaviour _aggressiveMoveBehaviour;

    private float _distToPlayer => Vector2.Distance(transform.position, _player.transform.position);

    private void OnEnable()
    {
        _passiveMoveBehaviour.Initialize(_player);
        _aggressiveMoveBehaviour.Initialize(_player);

        _passiveMoveBehaviour.OnMove += HandleMove;
        _aggressiveMoveBehaviour.OnMove += HandleMove;
    }

    private void OnDisable()
    {
        _passiveMoveBehaviour.OnMove -= HandleMove;
        _aggressiveMoveBehaviour.OnMove -= HandleMove;
    }

    private void Update()
    {
        if(_distToPlayer < _detectionRange)
        {
            _aggressiveMoveBehaviour.PerformBehaviour();
            _spr.color = Color.red;
        }
        else
        {
            _passiveMoveBehaviour.PerformBehaviour();
            _spr.color = Color.green;
        }
    }

    private void HandleMove(EnemyMovementBehaviour moveBehaviour, Vector2 moveDir)
    {
        _anim.SetTrigger("Move");
        _spr.transform.rotation = Quaternion.Euler(new Vector3(0,0, (Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg) - 90f));
    }
}
