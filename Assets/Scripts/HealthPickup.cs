using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPool? hp = collision.gameObject.GetComponent<HealthPool>();

        if (hp != null)
        {
            hp.Heal(1);

            Destroy(gameObject);
        }
    }
}
