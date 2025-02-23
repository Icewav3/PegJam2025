using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private bool _firing = false;
    private bool _canFire = true;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private GameObject _projectile;

    [SerializeField]
    private PlayerAim _aimer;

    [SerializeField]
    private DamageReceiver _dr;
    
    public void StartFire()
    {
        if (_firing) return;
        if (!_canFire) return;

        _firing = true;
        _canFire = false;
        _anim.SetTrigger("Fire");
    }

    public void StopFire()
    {
        _anim.ResetTrigger("Fire");
        _firing = false;
        _canFire = true;
    }

    private void Update()
    {
        if(!_firing && _anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerHoldFire"))
        {
            _anim.SetTrigger("ReleaseFire");
            Instantiate(_projectile, transform.position, _aimer.transform.rotation);
            _dr.ReceiveDamage(new DamageEvent(1, gameObject, gameObject));
        }
    }
}
