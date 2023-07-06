using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool activated;
    public AudioSource AudioSource;
    public GameObject ClipON;
    public GameObject ClipOFF;

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
            activated = false;
            Time.timeScale = 1f;
            GameObject Sound = Instantiate(ClipON);
            Destroy(Sound, 1f);
            gameObject.SetActive(false);
        }
        else
        {
            activated = true;
            gameObject.SetActive(true);
            GameObject Sound = Instantiate(ClipOFF);
            Destroy(Sound, 1f);
            Time.timeScale = 0f;
        }
    }

    public void HomeButton()
    {
        PauseAux();
        GameManager.Instance.LoadMainMenu();
    }

}
