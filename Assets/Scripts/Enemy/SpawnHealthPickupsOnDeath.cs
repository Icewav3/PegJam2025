using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHealthPickupsOnDeath : MonoBehaviour
{
    [SerializeField]
    private DamageReceiver _dr;
    [SerializeField]
    private HealthPickup _pickup;

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
        for(int i = 0; i < _dr.HealthPool.StartingHealth; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 2;
            HealthPickup hp = Instantiate(_pickup, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }
}
