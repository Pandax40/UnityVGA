using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void Start()
    {

    }
    public void HomeButton()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
