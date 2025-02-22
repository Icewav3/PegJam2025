using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithHealth : MonoBehaviour
{
    [SerializeField]
    private HealthPool _hp;

    private void Update()
    {
        transform.localScale = Vector3.one * (1 + (0.1f * (_hp.Health - _hp.StartingHealth)));
    }
}
