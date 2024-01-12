using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform _cameraPosition;
    Transform _transform;
    Vector3 _initialPosition;

    private void Start()
    {
        _transform = transform;
        _cameraPosition = GameObject.Find("CameraPosition").GetComponent<Transform>();
        _initialPosition = _transform.position;
    }
    private void Update()
    {
        _transform.position = Vector3.Lerp(_transform.position, _cameraPosition.position, 60 * Time.deltaTime);
    }
}
