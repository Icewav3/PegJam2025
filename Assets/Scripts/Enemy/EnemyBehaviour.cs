using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected GameObject _player;

    public void Initialize(GameObject player)
    {
        _player = player;
    }

    public abstract void PerformBehaviour();
}
