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
    [SerializeField]
    private Animator _secondaryAnim;

    private float _timeToLunge = 0;

    private bool _moving;
    private Vector2 _lastMoveInput;

    public void HandleMove(Vector2 moveInfo)
    {
        _lastMoveInput = moveInfo;
    }

    public void StartMove()
    {
        _moving = true;
    }

    public void StopMove()
    {
        _moving = false;
    }

    private void Update()
    {
        _timeToLunge -= Time.deltaTime;

        if(_timeToLunge < 0 && _moving)
        {
            _timeToLunge = _lungeDuration;
            _rb.AddForce(_lastMoveInput * _lungeForce);
            _secondaryAnim.SetTrigger("Move");
        }
    }
}