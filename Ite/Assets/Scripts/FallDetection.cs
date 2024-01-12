using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FallDetection : MonoBehaviour
{
    Transform _transform;
    LifeManager _lifeManager;

    private void Start()
    {
        _transform = transform;
        _lifeManager = GetComponent<LifeManager>();
    }
    private void Update()
    {
        if(_transform.position.y < -10)
        {
            _transform.position = Vector3.zero + Vector3.up;
            _lifeManager.Hit();
        }
    }
}
