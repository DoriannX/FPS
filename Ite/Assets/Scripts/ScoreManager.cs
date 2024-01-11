using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public int score = 0;
    TextMeshProUGUI _scoreText;

    private void Start()
    {
        _scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _scoreText.text = "Enemy killed : " + score.ToString();
    }
}
