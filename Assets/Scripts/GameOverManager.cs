using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void HomeButton()
    {
        SceneManager.LoadScene(0);
    }
}
