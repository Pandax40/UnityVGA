using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool activated;
    public AudioSource AudioSource;
    public AudioClip PauseON;
    public AudioClip PauseOFF;

    void Start()
    {
        activated = true;
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if(SceneManager.GetActiveScene().buildIndex > 1 && ctx.performed)
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
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            AudioSource.PlayOneShot(PauseON, 3f);
            activated = true;
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void HomeButton()
    {
        PauseAux();
        GameManager.Instance.LoadMainMenu();
    }

}
