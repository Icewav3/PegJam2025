using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithHealth : MonoBehaviour
{
    [SerializeField]
    private HealthPool _hp;
    [SerializeField]
    private float _baseScale = 1f;

    private void Update()
    {
        transform.localScale = Vector3.one * (_baseScale + (0.1f * (_hp.Health - _hp.StartingHealth)));
    }
}
