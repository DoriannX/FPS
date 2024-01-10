using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    Animator _animator;
    [SerializeField] LayerMask _projectileLayer;

    CapsuleCollider _cC;
    Rigidbody _rb;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _cC = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            _animator.SetBool("touched", true);
            _cC.isTrigger = true;
            _rb.isKinematic = true;
        }
    }
}
