using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float _time;
    DeathManager _deathManager;
    float _currentTimeRemaining;
    TextMeshProUGUI _timeText;

    private void Start()
    {
        _deathManager = GameObject.Find("Manager").GetComponent<DeathManager>();
        Invoke(nameof(Death), _time);
        _currentTimeRemaining = _time;
        _timeText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(_currentTimeRemaining > 0)
        {
            _currentTimeRemaining -= Time.deltaTime;
        }
        _timeText.text = "time left : " + ((int)_currentTimeRemaining).ToString();
    }

    void Death()
    {
        _deathManager.Dead();   
    }
}
