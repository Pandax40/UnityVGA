using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject ControlScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play()
    {
        gameObject.SetActive(false);
        ControlScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
