using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyShoot : MonoBehaviour
{
    [Header("References")]
    Transform _attackPoint;
    List<Object> _objectsToThrow = new List<Object>();
    Object _objectToThrow;

    [Header("Settings")]
    [SerializeField] int _totalThrows;
    [SerializeField] float _throwCooldown;

    [Header("Throwing")]
    [SerializeField] float _throwForce;
    [SerializeField] float _throwUpwardForce;

    bool _readyToThrow = true;

    Transform _transform;

    [SerializeField] LayerMask _playerLayer;
    Vector3 _forceToAdd;

    playerDetecter _playerDetecter;
    GameObject _player;
    NavMeshAgent _agent;

    private void Start()
    {
        _transform = transform;
        _attackPoint = _transform;
        _objectsToThrow = Resources.LoadAll("Projectiles/").ToList();
        foreach (Transform child in _transform)
        {
            if (child.name == "PlayerDetecter")
            {
                _playerDetecter = child.gameObject.GetComponent<playerDetecter>();
            }
        }
        _player = GameObject.Find("Player");
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (_playerDetecter._playerDetected && _readyToThrow && _totalThrows > 0 && _agent)
        {
            Throw();
        }
    }


    void Throw()
    {
        _readyToThrow = false;

        //instantiate object to throw
        _objectToThrow = _objectsToThrow[Random.Range(0, _objectsToThrow.Count)];
        GameObject projectile = Instantiate(_objectToThrow, _attackPoint.position + _transform.up + _transform.forward, _transform.rotation) as GameObject;
        projectile.layer = 0;
        //calculate direction
        Vector3 forceDirection = (_player.transform.position - _player.transform.up) - projectile.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(_transform.position, _transform.forward, out hit, 500f, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Projectile") | 1 << LayerMask.NameToLayer("no"))))
        {
            forceDirection = (hit.point - _attackPoint.position).normalized;
            print(hit.collider.gameObject.layer);
        }

        //Calculate force to add
        _forceToAdd = forceDirection * _throwForce + _transform.up * _throwUpwardForce;
        if (projectile.GetComponent<Collider>() is MeshCollider meshcolid)
            meshcolid.convex = true;


        AddForce(projectile.transform);
        if (projectile.tag != "articulated")
        {
            Rigidbody parentBody = projectile.AddComponent<Rigidbody>();
            parentBody.interpolation = RigidbodyInterpolation.Interpolate;
            parentBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            parentBody.AddForce(_forceToAdd, ForceMode.Impulse);
        }

        DestroyChair parentDestroy = projectile.AddComponent<DestroyChair>();
        parentDestroy._layerToStopExclude = 8;
        _totalThrows--;
        //implement throw cooldown
        Invoke(nameof(ResetThrow), _throwCooldown);
    }

    private void AddForce(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("Enfant trouv� : " + child.name);
            if (child.TryGetComponent<Rigidbody>(out Rigidbody body))
            {
                body.interpolation = RigidbodyInterpolation.Interpolate;
                body.collisionDetectionMode = CollisionDetectionMode.Continuous;
                body.AddForce(_forceToAdd, ForceMode.Impulse);
            }
            AddForce(child);
        }
    }


    void ResetThrow()
    {
        _readyToThrow = true;
    }
}
