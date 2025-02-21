using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _lungeForce = 5f;
    [SerializeField]
    private float _lungeDuration = 0.5f;

    private Coroutine _moveRoutine;
    private Vector2 _lastMoveInput;

    public void HandleMove(Vector2 moveInfo)
    {
        _lastMoveInput = moveInfo;
    }

    public void StartMove()
    {
        _rb.AddForce(_lastMoveInput * _lungeForce);

        _moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void StopMove()
    {
        StopCoroutine(_moveRoutine);
        _moveRoutine = null;
    }

    private IEnumerator MoveRoutine()
    {
        while(true)
        {
            _rb.AddForce(_lastMoveInput * _lungeForce);

            yield return new WaitForSeconds(_lungeDuration);
        }
    }
}