using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public event Action<EnemyBrain, DamageReceiver, DamageEvent> OnDeath;

    private GameObject _player => SceneGod.SInstance.player;

    [SerializeField]
    private SpriteRenderer _spr;

    [SerializeField]
    private Color _passiveColor;

    [SerializeField]
    private Color _aggroColor;

    [SerializeField]
    private DamageReceiver _dr;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private float _detectionRange = 5f;

    [SerializeField]
    private EnemyMovementBehaviour _passiveMoveBehaviour;

    [SerializeField]
    private EnemyMovementBehaviour _aggressiveMoveBehaviour;

    [SerializeField]
    private int _scoreValue;
    public int ScoreValue => _scoreValue;

    private float _distToPlayer => Vector2.Distance(transform.position, _player.transform.position);

    public bool InRange => _distToPlayer < _detectionRange;

    private void OnEnable()
    {
        _passiveMoveBehaviour.Initialize(_player);
        _aggressiveMoveBehaviour.Initialize(_player);

        _passiveMoveBehaviour.OnMove += HandleMove;
        _aggressiveMoveBehaviour.OnMove += HandleMove;

        _dr.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _passiveMoveBehaviour.OnMove -= HandleMove;
        _aggressiveMoveBehaviour.OnMove -= HandleMove;

        _dr.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        if(InRange)
        {
            _aggressiveMoveBehaviour.PerformBehaviour();
            _spr.color = _aggroColor;
        }
        else
        {
            _passiveMoveBehaviour.PerformBehaviour();
            _spr.color = _passiveColor;
        }
    }

    private void HandleMove(EnemyMovementBehaviour moveBehaviour, Vector2 moveDir)
    {
        _anim.SetTrigger("Move");
        _spr.transform.rotation = Quaternion.Euler(new Vector3(0,0, (Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg) - 90f));
    }

    private void HandleDeath(DamageReceiver dr, DamageEvent dmgEvent)
    {
        OnDeath?.Invoke(this, dr, dmgEvent);
    }
}
