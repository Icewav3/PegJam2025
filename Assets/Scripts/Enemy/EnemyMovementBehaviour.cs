using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovementBehaviour : MonoBehaviour
{
    protected GameObject _player;

    public event Action<EnemyMovementBehaviour, Vector2> OnMove;

    public void Initialize(GameObject player)
    {
        _player = player;
    }

    public abstract void PerformBehaviour();

    protected void BroadcastMove(Vector2 moveDir)
    {
        OnMove?.Invoke(this, moveDir);
    }
}
