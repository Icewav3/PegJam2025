using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private PlayerMove _playerMove;

    [SerializeField]
    private PlayerFire _playerFire;

    private void OnEnable()
    {
        _playerInput.actions.FindAction("Move").started += StartMove;
        _playerInput.actions.FindAction("Move").performed += HandleMove;
        _playerInput.actions.FindAction("Move").canceled += StopMove;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Move").started -= StartMove;
        _playerInput.actions.FindAction("Move").performed -= HandleMove;
        _playerInput.actions.FindAction("Move").canceled -= StopMove;
    }

    private void StartMove(InputAction.CallbackContext ctx)
    {
        _playerMove.StartMove();
    }

    private void HandleMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(ctx.ReadValue<Vector2>());
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(Vector2.zero);
        _playerMove.StopMove();
    }
}
