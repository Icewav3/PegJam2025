using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnHealthOrbs : MonoBehaviour
{
    [SerializeField]
    private HealthPool _healthPool;
    [SerializeField]
    private GameObject _healthOrb;

    private List<GameObject> _orbs = new List<GameObject>();

    private void OnEnable()
    {
        _healthPool.OnHPChange += HandleHPChange;

        for(int i = 0; i < _healthPool.StartingHealth; i++)
        {
            SpawnHPOrb();
        }
    }

    private void OnDisable()
    {
        _healthPool.OnHPChange -= HandleHPChange;
    }

    private void SpawnHPOrb()
    {
        GameObject hpOrb = Instantiate(_healthOrb, transform.position, Quaternion.identity, transform);
        _orbs.Add(hpOrb);
    }

    private void HandleHPChange(HealthPool pool, int change)
    {
        if(change < 0)
        {
            Destroy(_orbs.First());
            _orbs.Remove(_orbs.First());
        }
        else
        {
            SpawnHPOrb();
        }
    }
}
