using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPool : MonoBehaviour
{
    public event Action<HealthPool, float> OnHPChange;

    [SerializeField]
    private int _startingHealth;
    public int StartingHealth => _startingHealth;

    private int _health;
    public int Health => _health;

    private void OnEnable()
    {
        _health = _startingHealth;
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
        Debug.Log(_health);
        OnHPChange?.Invoke(this, damage);
        return damage;
    }

    public int Heal(int heal)
    {
        _health += heal;

        OnHPChange?.Invoke(this, heal);

        return heal;
    }
}
