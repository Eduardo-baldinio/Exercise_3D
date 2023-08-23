using UnityEngine;
[RequireComponent (typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float _animSpeed;

    [HideInInspector] public bool IsJump;
    [HideInInspector] public bool IsSprint;
    [HideInInspector] public bool IsCrouch;
    [HideInInspector] public bool DontJump;

    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private float _playerSprint = 7f;
    [SerializeField] private float _playerCrouch = 2f;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private float _rotationSpeed = 1f;
    private float _gravityValue = -9.81f;

    private bool _isGround;

    private Vector3 _playerVelosity;
    private Vector3 _move;
    private Vector3 _moveInput;

    public Vector3 MoveInput
    {
        get
        {
            return _moveInput;
        }

        set
        {
            _moveInput.x = value.x;
            _moveInput.z = value.y;
        }
    }

    private CharacterController _characterController;
    private Transform _cameraMainTransform;
   
    private void Awake()
    {
        _cameraMainTransform = Camera.main.transform;
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        Move();
        Jump();
        PlayerRotation();
    }

    private void Move()
    {
        if (_isGround && _playerVelosity.y < 0)
        {
            _playerVelosity.y = 0;
            DontJump = false;
        }

        _move = _cameraMainTransform.forward * _moveInput.z + _cameraMainTransform.right * _moveInput.x;

        if (IsSprint)
        {
            _characterController.Move(_move * Time.deltaTime * _playerSprint);
            _animSpeed = _playerSprint;

        } else if (IsCrouch)
        {
            _characterController.Move(_move * Time.deltaTime * _playerCrouch);
            _animSpeed = _playerCrouch;
        }
        else
        {
            _characterController.Move(_move * Time.deltaTime * _playerSpeed);
            _animSpeed = _playerSpeed;
        }

        _playerVelosity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelosity * Time.deltaTime);

    }

    private void Jump()
    {
        _isGround = _characterController.isGrounded;

        if (_isGround && IsJump)
        {
            _playerVelosity.y = Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
            IsJump = false;
            DontJump = true;
        }
    }

    private void PlayerRotation()
    {
        if (_moveInput != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(_moveInput.x, _moveInput.z) * Mathf.Rad2Deg + _cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }
    }
}
