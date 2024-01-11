using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetecter : MonoBehaviour
{
    public bool _playerDetected { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            _playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            _playerDetected = false;
        }
    }
}
