using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    Transform _cam;
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

    bool _throwInput;

    Transform _tranform;

    [SerializeField] LayerMask _playerLayer;
    Vector3 _forceToAdd;

    private void Start()
    {
        _cam = GameObject.Find("Main Camera").transform;
        _tranform = transform;
        _attackPoint = GameObject.Find("ThrowPointAttack").transform;
        _objectsToThrow = Resources.LoadAll("Projectiles/").ToList();
    }
    
    public void gatherThrow(InputAction.CallbackContext ctx)
    {
        if(_readyToThrow && _totalThrows > 0)
        {
            Throw();
        }   
    }


    void Throw()
    {
        _readyToThrow = false;

        //instantiate object to throw
        _objectToThrow = _objectsToThrow[Random.Range(0, _objectsToThrow.Count)];
        GameObject projectile = Instantiate(_objectToThrow, _attackPoint.position, _cam.rotation) as GameObject;
        projectile.layer = 9;
        //calculate direction
        Vector3 forceDirection = _cam.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(_cam.position, _cam.forward, out hit, 500f, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Projectile") | 1 << LayerMask.NameToLayer("no"))))
        {
            forceDirection = (hit.point - _attackPoint.position).normalized;
            print(hit.collider.gameObject.layer);
        }

        //Calculate force to add
        _forceToAdd = forceDirection * _throwForce + _tranform.up * _throwUpwardForce;
        if (projectile.GetComponent<Collider>() is MeshCollider meshcolid)
            meshcolid.convex = true;


        AddForce(projectile.transform);
        Rigidbody parentBody = projectile.AddComponent<Rigidbody>();
        DestroyChair parentDestroy = projectile.AddComponent<DestroyChair>();
        parentDestroy._layerToStopExclude = 8;
        parentBody.interpolation = RigidbodyInterpolation.Interpolate;
        parentBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        parentBody.AddForce(_forceToAdd, ForceMode.Impulse);
        _totalThrows--;

        //implement throw cooldown
        Invoke(nameof(ResetThrow), _throwCooldown);
    }

    private void AddForce(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("Enfant trouvé : " + child.name);
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
