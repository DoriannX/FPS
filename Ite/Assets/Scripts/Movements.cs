using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    //declare variables
    [Header("Movements")]
    float _moveSpeed;
    [SerializeField] float _walkSpeed;
    [SerializeField] float _sprintSpeed;

    [SerializeField] float _groundDrag;

    [Header("Jumping")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _jumpCooldown;
    [SerializeField] float _airMultiplier;

    bool _readyToJump = true;

    [Header("Crouching")]
    [SerializeField] float _crouchSpeed;
    [SerializeField] float _crouchYScale;
    float _startYScale;

    [Header("Ground Checks")]
    [SerializeField] float _playerHeight;
    [SerializeField] LayerMask _whatIsGround;
    bool _grounded = false;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit _slopeHit;
    bool _exitingSlope = false;

    bool _jumpInput = false;
    bool _sprintInput = false;
    bool _crouchInput = false;

    Transform _orientation;

    float _horizontalInput;
    float _verticalInput;

    Vector3 _moveDirection;

    Rigidbody _rb;

    Transform _transform;

    MovementState _state;

    enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        //Assign rigidbody
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        //Assign orientation
        _orientation = GameObject.Find("Orientation").transform;
        _transform = transform;
        _startYScale = _transform.localScale.y;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        //ground check
        _grounded = Physics.Raycast(_transform.position, Vector3.down, _playerHeight * 0.5f + .2f, _whatIsGround);
        
        //handle drag
        _rb.drag = (_grounded) ? _groundDrag : 0f;

        SpeedControl();
        if(_jumpInput)
        {
            if (_readyToJump && _grounded)
            {
                _readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), _jumpCooldown);
            }
        }

        StateHandler();
    }
    public void GatherInput(InputAction.CallbackContext ctx)
    {
        //gather inputs
        _horizontalInput = ctx.ReadValue<Vector2>().x;
        _verticalInput = ctx.ReadValue<Vector2>().y;
        print(ctx.ReadValue<Vector2>());
        print(_horizontalInput);
        print(_verticalInput);
    }

    void StateHandler()
    {
        if (_crouchInput)
        {
            _state = MovementState.crouching;
            _moveSpeed = _crouchSpeed;
        }

        //sprinting
        else if(_grounded && _sprintInput)
        {
            _state = MovementState.sprinting;
            _moveSpeed = _sprintSpeed;
        }

        else if (_grounded)
        {
            _state = MovementState.walking;
            _moveSpeed = _walkSpeed;
        }

        else
        {
            _state = MovementState.air;
        }

        //turn gravity off on slope
    }
    void MovePlayer()
    {
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        //on slope
        if (OnSlope() && !_exitingSlope)
        {

            _rb.AddForce(GetSlopeMoveDirection() * _moveSpeed * 20f, ForceMode.Force);
            print("on slope");
            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //grounded
        else if (_grounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        //not grounded
        else if(!_grounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);

        _rb.useGravity = !OnSlope();

    }

    void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !_exitingSlope)
        {
            if(_rb.velocity.magnitude > _moveSpeed)
            {
                _rb.velocity = _rb.velocity.normalized * _moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            //limit the speed
            if (flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;

                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        _exitingSlope = true;
        print("jump");
        //reset y velocity
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        //Apply jump
        _rb.AddForce(_transform.up * _jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        _readyToJump = true;
        _exitingSlope = false;
    }

    public void GatherJump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            _jumpInput = true;
        else if(ctx.canceled)
            _jumpInput = false;
    }

    public void GatherSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            _sprintInput = true;
        else if (ctx.canceled)
            _sprintInput = false;
    }

    public void GatherCrouch(InputAction.CallbackContext ctx)
    {
        _crouchInput = !(_crouchInput);
        //crouching
        if (_crouchInput)
        {
            _transform.localScale = new Vector3(_transform.localScale.x, _crouchYScale, _transform.localScale.z);
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
        {
            _transform.localScale = new Vector3(_transform.localScale.x, _startYScale, _transform.localScale.z);
        }
    }

    bool OnSlope()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, out _slopeHit, _playerHeight * .5f + .3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }
}
