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
    Animator _animator;
    EnemyDeath _enemyDeath;

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
        _animator = GetComponent<Animator>();
        _enemyDeath = GetComponent<EnemyDeath>();
    }

    private void Update()
    {
        if (_agent)
        {
            
            if (!_playerDetecter._playerDetected)
            {
                _agent.destination = _player.transform.position;
                _transform.position = _agent.nextPosition;
            }
        }

        print(_rb.velocity.magnitude);
        if (_animator)
        {
            if (_agent.velocity.magnitude >= 0.5f)
            {
                print("walk");
                _animator.SetBool("isWalking", true);
            }
            else if (_enemyDeath.dead)
            {
                print("dead");
                Destroy(_animator);
            }
            else if(_agent.velocity.magnitude <= .5f)
            {
                print("idle");
                _animator.SetBool("isWalking", false);
                _transform.LookAt(new Vector3(_player.transform.position.x, _transform.position.y, _player.transform.position.z));
            }
        }

    }
   


}
