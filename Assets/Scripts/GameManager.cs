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

    //TODO:
    // - Una array de las propiedades del pesonaje
    // - Array de tiempos.
    // - Array de structs con las propiedades del mapa y probabilidades
    // - Guardar el estado del juego.
    // IMPORANTE: Que a partir de estos valores el mapa y el jugador no dependa de nada mas.

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
        Platform.Probability = 10;
        Monedas = 3;
        Health = 3;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    public void AddCoin()
    {
        Monedas++;
        Interfaz.UpdateScreen();
    }
}
