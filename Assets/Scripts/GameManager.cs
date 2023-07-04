using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public static int Monedas { get; private set; }
    public GameObject Player { get; private set; }
    public GameObject UI { get; private set; }
    public int Health { get; private set; } 

    [SerializeField] private GameObject PlayerScene;
    [SerializeField] private GameObject Interfaz;

    void Awake()
    {
        if (Instance == null)
        {
            Destroy(gameObject);
        }
        Monedas = 3;
        Health = 3;
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin()
    {
        Monedas++;
    }
}
