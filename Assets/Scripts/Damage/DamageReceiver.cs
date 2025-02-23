using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTeam
{
    Enemy,
    Player,
    Neutral
}

public class DamageReceiver : MonoBehaviour
{
    public event Action<DamageReceiver, DamageEvent> OnDamage;
    public event Action<DamageReceiver, DamageEvent> OnDeath;

    [SerializeField]
    private DamageTeam _team;
    public DamageTeam Team => _team;

    [SerializeField]
    private HealthPool _healthPool;
    public HealthPool HealthPool => _healthPool;

    [SerializeField]
    private bool _destroyOnDeath = false;

    public void ReceiveDamage(DamageEvent dmgEvent)
    {
        if (_healthPool.Health <= 0) return;

        int damage = dmgEvent.Damage;

        damage -= _healthPool.Damage(damage);

        OnDamage?.Invoke(this, dmgEvent);

        if (_healthPool.Health <= 0)
        {
            OnDeath?.Invoke(this, dmgEvent);
            if(_destroyOnDeath) Destroy(gameObject);
        }
    }
}

public class DamageEvent
{
    public DamageEvent(int damage, GameObject mainSource, GameObject specificSource)
    {
        Damage = damage;
        MainSource = mainSource;
        SpecificSource = specificSource;
    }   

    public int Damage { get; }
    public GameObject MainSource { get; }
    public GameObject SpecificSource { get; }
}