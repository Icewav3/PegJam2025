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

    [SerializeField]
    private PlayerAim _playerAim;

    [SerializeField]
    private Camera _camera;

    private void OnEnable()
    {
        if (_camera == null) _camera = Camera.main;

        _playerInput.actions.FindAction("Move").started += StartMove;
        _playerInput.actions.FindAction("Move").performed += HandleMove;
        _playerInput.actions.FindAction("Move").canceled += StopMove;

        _playerInput.actions.FindAction("Fire").started += StartFire;
        _playerInput.actions.FindAction("Fire").canceled += StopFire;

        _playerInput.actions.FindAction("Aim").performed += HandleAim;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Move").started -= StartMove;
        _playerInput.actions.FindAction("Move").performed -= HandleMove;
        _playerInput.actions.FindAction("Move").canceled -= StopMove;

        _playerInput.actions.FindAction("Fire").started -= StartFire;
        _playerInput.actions.FindAction("Fire").canceled -= StopFire;

        _playerInput.actions.FindAction("Aim").performed -= HandleAim;
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

    private void StartFire(InputAction.CallbackContext ctx)
    {
        _playerFire.StartFire();
    }

    private void StopFire(InputAction.CallbackContext ctx)
    {
        _playerFire.StopFire();
    }

    private void HandleAim(InputAction.CallbackContext ctx)
    {
        _playerAim.HandleAim(_camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>()));
    }
}
