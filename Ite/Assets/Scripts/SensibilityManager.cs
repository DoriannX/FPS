using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensibilityManager : MonoBehaviour
{
    Slider _sensibilite;
    float sensitivity = 0;
    TextMeshProUGUI _sensibilityValue;

    private void Start()
    {
        _sensibilite = GetComponent<Slider>();
        if(GameObject.Find("SensibilityValue"))
            _sensibilityValue = GameObject.Find("SensibilityValue").GetComponent<TextMeshProUGUI>();
        sensitivity = PlayerPrefs.GetFloat("sensitivity");
        if (_sensibilite)
        {
            _sensibilite.value = PlayerPrefs.GetFloat("sensitivity");
        }
    }
    private void Update()
    {
        if (_sensibilityValue && _sensibilite)
        {
            sensitivity = _sensibilite.value;
            PlayerPrefs.SetFloat("sensitivity", sensitivity);
            _sensibilityValue.text = sensitivity.ToString();
        }
    }
}
