using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerDeath : MonoBehaviour
{
    [SerializeField]
    private DamageReceiver _dr;

    [SerializeField]
    private SpriteRenderer _spr;

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
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        _spr.enabled = false;

        yield return new WaitForSeconds(2);

        SceneGod.SInstance.IncrementDeaths();
    }
}
