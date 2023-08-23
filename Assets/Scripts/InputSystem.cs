using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    #region InputSystem

    private PlayerInput _inputController;
    private InputAction _actionMove;
    private InputAction _actionJump;
    private InputAction _actionSprint;
    private InputAction _actionCrouch;

    #endregion

    private PlayerController _playerController;
    private Animator _animator;

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();

        _inputController = GetComponent<PlayerInput>();
        _actionMove = _inputController.actions["Move"];
        _actionJump = _inputController.actions["Jump"];
        _actionSprint = _inputController.actions["Sprint"];
        _actionCrouch = _inputController.actions["Crouch"];

    }

    void Update()
    {
        ActionMove();
        ActionJump();
        ActionSprint();
        ActionCrouch();
    }

    private void ActionMove()
    {
        Vector2 inputMoveAction = _actionMove.ReadValue<Vector2>();
        _playerController.MoveInput = inputMoveAction;

        if (inputMoveAction != Vector2.zero)
        {
            _animator.SetBool("slow run", true);
        }
        else
        {
            _animator.SetBool("slow run", false);
        }
    }

    private void ActionJump()
    {
        if (_actionJump.triggered && !_playerController.DontJump)
        {
            _playerController.IsJump = true;
            _animator.SetTrigger("jump");
        }
    }

    private void ActionSprint()
    {
        if (_actionSprint.inProgress && !_playerController.IsCrouch)
        {
            _playerController.IsSprint = true;
            _animator.SetBool("sprint", true);
        }
        else
        {
            _playerController.IsSprint = false;
            _animator.SetBool("sprint", false);
        }
    }

    private void ActionCrouch()
    {
        if (_actionCrouch.inProgress)
        {
            _playerController.IsCrouch = true;
            _animator.SetBool("crouch", true);
        }
        else
        {
            _playerController.IsCrouch= false;
            _animator.SetBool("crouch", false);
        }
    }
}
