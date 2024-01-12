using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    // Instantiation of the variables
    Transform _player;
    float _sensitivity = 2;
    [SerializeField] float _controllerSensitivity = .5f;
    float _yRotation = 0f;
    float _xRotation = 0f;
    //float _cameraVerticalRotation = 0f; 
    Vector3 _cameraPosition = Vector2.zero;
    Transform _transform;
    Transform _orientation;

    private void Start()
    {
        _transform = transform;
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _orientation = GameObject.Find("Orientation").transform;
    }

    private void Update()
    {
        _sensitivity = PlayerPrefs.GetFloat("sensitivity");
        _yRotation += _cameraPosition.x * Time.deltaTime;
        _xRotation -= _cameraPosition.y * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        _transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    public void GetCamerainput(InputAction.CallbackContext ctx)
    {
        _cameraPosition = (ctx.control.device is Mouse) ? ctx.ReadValue<Vector2>() * _sensitivity * 10f: ctx.ReadValue<Vector2>() * _controllerSensitivity * 10f;
    }
}
