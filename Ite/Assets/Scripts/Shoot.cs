using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    Transform _cam;
    Transform _attackPoint;
    [SerializeField] GameObject _objectToThrow;

    [Header("Settings")]
    [SerializeField] int _totalThrows;
    [SerializeField] float _throwCooldown;

    [Header("Throwing")]
    [SerializeField] float _throwForce;
    [SerializeField] float _throwUpwardForce;

    bool _readyToThrow = true;

    bool _throwInput;

    Transform _tranform;

    private void Start()
    {
        _cam = GameObject.Find("Main Camera").transform;
        _tranform = transform;
        _attackPoint = GameObject.Find("ThrowPointAttack").transform;
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
        GameObject projectile = Instantiate(_objectToThrow, _attackPoint.position, _cam.rotation);

        //get rb
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //calculate direction
        Vector3 forceDirection = _cam.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(_cam.position, _cam.forward, out hit, 500f, ~(1 << LayerMask.NameToLayer("Player")))){
            forceDirection = (hit.point - _attackPoint.position).normalized;
        }

        //add force
        Vector3 forceToAdd = forceDirection * _throwForce + _tranform.up * _throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        _totalThrows--;

        //implement throw cooldown
        Invoke(nameof(ResetThrow), _throwCooldown);
    }
    

    void ResetThrow()
    {
        _readyToThrow = true;
    }
}
