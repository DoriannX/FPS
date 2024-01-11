using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{
    //declare variables
    [Header("Movements")]
    [SerializeField] float _moveSpeed;

    [SerializeField] float _groundDrag;

    [Header("Ground Checks")]
    [SerializeField] float _playerHeight;
    [SerializeField] LayerMask _whatIsGround;
    bool _grounded = false;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit _slopeHit;
    bool _exitingSlope = false;

    [Header("In air")]
    [SerializeField] float _airMultiplier;

    Vector3 _moveDirection;

    Rigidbody _rb;

    GameObject _player;
    Transform _transform;
    playerDetecter _playerDetecter;
    EnemyCollisionDetection _enemyCollisionDetection;
    NavMeshAgent _agent;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _transform = transform;
        _player = GameObject.Find("Player");
        foreach(Transform child in _transform)
        {
            if(child.name == "PlayerDetecter")
            {
                _playerDetecter = child.gameObject.GetComponent<playerDetecter>();
            }
        }

        _enemyCollisionDetection = GetComponent<EnemyCollisionDetection>();
        _agent = GetComponent<NavMeshAgent>();
        
    }
    private void FixedUpdate()
    {
        //if (_playerDetecter._playerDetected)
        //{
        //    return;
        //}
        //MoveEnemy();
    }

    private void Update()
    {
        ////ground check
        //_grounded = Physics.Raycast(_transform.position, Vector3.down, _playerHeight * 0.5f + .2f, _whatIsGround);

        ////handle drag
        //_rb.drag = (_grounded) ? _groundDrag : 0f;

        //SpeedControl();

        
        if (_agent)
        {
            _transform.LookAt(new Vector3(_player.transform.position.x, _transform.position.y, _player.transform.position.z));
            if (!_playerDetecter._playerDetected)
            {
                _agent.destination = _player.transform.position;
                _transform.position = _agent.nextPosition;
            }
        }

    }
    void MoveEnemy()
    {
        if (_enemyCollisionDetection.obstacleDetected)
        {
            transform.Rotate(Vector3.up);
        }
        else
        {
            transform.LookAt(_player.transform.position);
        }
        _moveDirection = _transform.forward;
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
        {
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
            print("grounded");
        }
        //not grounded
        else if (!_grounded)
        {
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
            print("not grounded");
        }

        _rb.useGravity = !OnSlope();

    }

    void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > _moveSpeed)
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
