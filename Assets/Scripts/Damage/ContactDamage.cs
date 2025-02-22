using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField]
    private DamageTeam _team;
    [SerializeField]
    private int _damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
