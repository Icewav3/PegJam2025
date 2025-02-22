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

    [SerializeField]
    private DamageTeam _team;
    public DamageTeam Team => _team;

    [SerializeField]
    private HealthPool _healthPool;

    public void ReceiveDamage(DamageEvent dmgEvent)
    {
        float damage = dmgEvent.Damage;

        damage -= _healthPool.Damage(damage);

        OnDamage?.Invoke(this, dmgEvent);
    }
}

public class DamageEvent
{
    public float Damage { get; }
    public GameObject MainSource { get; }
    public GameObject SpecificSource { get; }
}