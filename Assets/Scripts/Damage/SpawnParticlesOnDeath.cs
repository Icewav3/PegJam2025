using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesOnDeath : MonoBehaviour
{
    [SerializeField]
    private DamageReceiver _dr;
    [SerializeField]
    private ParticleSystem _deathParticles;

    private void OnEnable()
    {
        _dr.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _dr.OnDeath -= HandleDeath;
    }

    private void HandleDeath(DamageReceiver dr, DamageEvent dmgEvent)
    {
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
    }
}
