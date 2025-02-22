#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField]
    private DamageTeam _team;
    [SerializeField]
    private int _damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageReceiver? dr = collision.collider.GetComponent<DamageReceiver>();

        if(dr != null && dr.Team != _team)
        {
            dr.ReceiveDamage(new DamageEvent(_damage, gameObject, gameObject));
        }
    }
}
