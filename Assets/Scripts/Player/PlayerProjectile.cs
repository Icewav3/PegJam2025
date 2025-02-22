using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _lifetime = 3;
    [SerializeField]
    private float _launchForce = 100;

    private void OnEnable()
    {
        _rb.AddForce(transform.up * _launchForce);

        Destroy(gameObject, _lifetime);
    }
}
