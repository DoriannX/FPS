using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    TMP_Dropdown _difficulty;

    private void Start()
    {
        _difficulty = GetComponent<TMP_Dropdown>();
        if (_difficulty)
            _difficulty.value = PlayerPrefs.GetInt("difficulty");

    }

    private void Update()
    {
        if(_difficulty)
            PlayerPrefs.SetInt("difficulty", _difficulty.value);
    }
}
