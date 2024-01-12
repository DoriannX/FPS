using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public int healthPoints { get; private set; } = 10;
    Image hitImage;
    DeathManager _deathManager;
    TextMeshProUGUI _lifeText;
    AudioSource _playerAudio;
    [SerializeField] AudioClip _stressFullMusic;
    [SerializeField] AudioClip _DeathMusic;
    bool stressed = false;
    bool dead = false;
    bool canBeHit = true;

    private void Start()
    {
        hitImage = GameObject.Find("hit").GetComponent<Image>();
        ResetHit();
        _deathManager = GameObject.Find("Manager").GetComponent<DeathManager>();
        _lifeText = GameObject.Find("Life").GetComponent<TextMeshProUGUI>();
        _playerAudio = GetComponent<AudioSource>();
    }
    public void Hit()
    {
        hitImage.enabled = true;
        Invoke(nameof(ResetHit), .3f);
        healthPoints--;
        print("hitted");
    }

    private void Update()
    {
        if(healthPoints <= 3 && !stressed)
        {
            _playerAudio.clip = _stressFullMusic;
            _playerAudio.Play();
            stressed = true;
        }
        if(healthPoints <= 0 && !dead)
        {
            _deathManager.Dead();
            AudioSource[] allAudioSources;
            allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audioS in allAudioSources)
            {
                audioS.Stop();
            }

            _playerAudio.clip = _DeathMusic;
            _playerAudio.Play();
            dead = true;
        }
        _lifeText.text = "HP : " + healthPoints.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.layer);
        if(collision.gameObject.layer == LayerMask.NameToLayer("ignore") && canBeHit)
        {
            canBeHit = false;
            Hit();
            Invoke(nameof(ResetDamage), 0.5f);
        }
    }

    void ResetHit()
    {
        hitImage.enabled = false;
    }

    void ResetDamage()
    {
        canBeHit = true;
    }
}
