using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPool : MonoBehaviour
{
    public event Action<HealthPool, float> OnHPChange;

    [SerializeField]
    private int _maxHealth;
    public float MaxHealth => _maxHealth;

    private int _health;
    public int Health => _health;

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public int Damage(int damage)
    {
        if(_health < damage)
        {
            damage -= _health;
            _health = 0;
        }
        else
        {
            _health -= damage;
        }

        OnHPChange?.Invoke(this, damage);
        return damage;
    }

    public int Heal(int heal)
    {
        if (_health + heal > _maxHealth)
        {
            heal -= (_maxHealth - _health);
            _health = _maxHealth;
        }
        else
        {
            _health += heal;
        }

        OnHPChange?.Invoke(this, heal);

        return heal;
    }
}
