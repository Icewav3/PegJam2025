using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaunchProjectiles : MonoBehaviour
{
    [SerializeField]
    private EnemyBrain _enemyBrain;

    [SerializeField]
    private PlayerProjectile _projectile;

    private float _timeToShoot;

    [SerializeField]
    private float _shotTime;

    [SerializeField]
    private float _aggroShotTime;

    [SerializeField]
    private float _minShots = 1;

    [SerializeField]
    private float _maxShots = 1;

    private void Update()
    {
        if(_timeToShoot <= 0)
        {
            if (_enemyBrain.InRange) _timeToShoot = _aggroShotTime;
            else _timeToShoot = _shotTime;

            for(int i = 0; i < Random.Range(_minShots, _maxShots); i++)
            {
                Instantiate(_projectile, transform.position, Quaternion.Euler(0,0,Random.Range(0,360)));
            }
        }

        _timeToShoot -= Time.deltaTime;
    }
}
