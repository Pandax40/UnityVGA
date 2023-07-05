using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool activated;
    public GameObject pausemenu;
    public AudioSource AudioSource;
    public AudioClip PauseON;
    public AudioClip PauseOFF;

    void Awake()
    {
        activated = false;
        pausemenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        PauseAux();
    }

    public void PlayButton()
    {
        PauseAux();
    }

    private void PauseAux()
    {
        if (activated)
        {
            AudioSource.PlayOneShot(PauseOFF, 3f);
            activated = false;
            pausemenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            AudioSource.PlayOneShot(PauseON, 3f);
            activated = true;
            pausemenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
