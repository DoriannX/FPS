using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _optionMenu;
    [SerializeField] GameObject _tutoMenu;
    LifeManager _lifeManager;
    private void Awake()
    {
        if( _pauseMenu != null )
            _pauseMenu.SetActive(false);
        if( _optionMenu != null )
            _optionMenu.SetActive(false);
        Time.timeScale = 0.0f;
        _lifeManager = GameObject.Find("Player").GetComponent<LifeManager>();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }
    public void PauseInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            Pause();
    }
    public void Pause()
    {
        if (!_optionMenu.activeSelf && !_tutoMenu.activeSelf && _lifeManager.healthPoints != 0)
        {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
            Time.timeScale = (Time.timeScale == 1.0f) ? 0 : 1.0f;
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }
        else if(!_tutoMenu.activeSelf && _lifeManager.healthPoints != 0)
        {
            _optionMenu.SetActive(!_optionMenu.activeSelf);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Option()
    {
        _optionMenu.SetActive(!_optionMenu.activeSelf);
    }

    public void Tuto()
    {
        _tutoMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
