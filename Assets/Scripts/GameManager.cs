using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public UI Interfaz { get; private set; }
    public int Monedas { get; private set; }
    public int Health { get; private set; } 

    [SerializeField] private GameObject PlayerScene;
    [SerializeField] private UI UIEstatica;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Player = PlayerScene;
        Interfaz = UIEstatica;
        Monedas = 3;
        Health = 3;
        Interfaz.UpdateScreen();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public void AddCoin()
    {
        Monedas++;
    }
}
