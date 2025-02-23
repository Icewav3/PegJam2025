#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    private int _scoreValue = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPool? hp = collision.gameObject.GetComponent<HealthPool>();

        if (hp != null)
        {
            hp.Heal(1);

            if(collision.gameObject.CompareTag("Player"))
            {
                SceneGod.SInstance.IncrementScore(_scoreValue);
            }

            Destroy(gameObject);
        }
    }
}
