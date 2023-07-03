using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameObject Player { get; private set; }

    [SerializeField] private GameObject PlayerScene;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        Player = PlayerScene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
