using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    Slider _volume;
    float volume = 0;
    TextMeshProUGUI _volumeManager;

    private void Start()
    {
        _volume = GetComponent<Slider>();
        if(GameObject.Find("VolumeValue"))
            _volumeManager = GameObject.Find("VolumeValue").GetComponent<TextMeshProUGUI>();
        if(_volume)
            _volume.value = PlayerPrefs.GetFloat("volume");
        volume = PlayerPrefs.GetFloat("volume");
        AudioListener.volume = volume / 100;
    }

    private void Update()
    {
        if(_volumeManager && _volume)
        {
            volume = _volume.value;
            PlayerPrefs.SetFloat("volume", volume);
            _volumeManager.text = volume.ToString();
            AudioListener.volume = volume / 100;
        }
    }
}
