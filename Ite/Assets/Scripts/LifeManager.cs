using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int healthPoints { get; private set; } = 10;
    Image hitImage;
    DeathManager _deathManager;
    TextMeshProUGUI _lifeText;

    private void Start()
    {
        hitImage = GameObject.Find("hit").GetComponent<Image>();
        ResetHit();
        _deathManager = GameObject.Find("Manager").GetComponent<DeathManager>();
        _lifeText = GameObject.Find("Life").GetComponent<TextMeshProUGUI>();
    }
    void Hit()
    {
        hitImage.enabled = true;
        Invoke(nameof(ResetHit), .3f);
        healthPoints--;
        print("hitted");
    }

    private void Update()
    {
        if(healthPoints <= 0)
        {
            _deathManager.Dead();
        }
        _lifeText.text = "HP : " + healthPoints.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.layer);
        if(collision.gameObject.layer == LayerMask.NameToLayer("ignore"))
        {
            Hit();
        }
    }

    void ResetHit()
    {
        hitImage.enabled = false;
    }
}
