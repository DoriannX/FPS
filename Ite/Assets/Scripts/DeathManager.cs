using TMPro;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    GameObject _death;
    TextMeshProUGUI _deathText;
    ScoreManager _scoreManager;

    private void Start()
    {
        _death = GameObject.Find("GameOver");
        _deathText = _death.GetComponentInChildren<TextMeshProUGUI>();
        _scoreManager = GameObject.Find("Manager").GetComponent<ScoreManager>();
        _death.SetActive(false);
    }

    public void Dead()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _death.SetActive(true);
        _deathText.text = "Game Over. Score :  " + _scoreManager.score;
        Time.timeScale = 0f;
        print("game over");
    }
}
