using System;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenManager : MonoBehaviour
{
    Toggle _fullScreen;

    private void Awake()
    {
        _fullScreen = GetComponent<Toggle>();
    }

    private void Update()
    {
        Screen.fullScreen = _fullScreen.isOn;
        //PlayerPrefs.SetInt("fullScreen", Convert.ToInt32(_fullScreen.isOn));
    }
}
