using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private bool _firing = false;

    [SerializeField]
    private Animator _anim;
    
    public void StartFire()
    {
        if (_firing) return;

        _firing = true;
        _anim.SetTrigger("Fire");
    }

    public void StopFire()
    {
        _firing = false;
    }

    private void Update()
    {
        if(!_firing && _anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerHoldFire"))
        {
            _anim.SetTrigger("ReleaseFire");
        }
    }
}
