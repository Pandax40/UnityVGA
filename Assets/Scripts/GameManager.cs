using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PlayerProp
{
    public PlayerProp(int hearts) 
    {
        runParticles = 0;
        plusJump = plusVelocity = dobleWallJump = dobleJump = false;
           
        extraHeart = true;
        this.hearts = hearts; 
    }
    public int runParticles; //Maps [0] Forest [1] Cave [2] Castle
    public int hearts { get; }
    public bool extraHeart { get; }
    public bool plusVelocity;
    public bool plusJump;
    public bool dobleJump;
    public bool dobleWallJump;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerProp[] PlayerRound;
    [SerializeField] private int[] CoinSpawnProbability;
    [SerializeField] private int[] DamageSysInterval;
    [SerializeField] private int[] DamageSysTimer;
    [SerializeField] private int[] mapIndexs;
    [SerializeField] private float[] timers;
    [SerializeField] private GameObject PlayerScene;
    [SerializeField] private UI UIEstatica;

    private int actualIndex;

    public GameObject Player { get => PlayerScene; }
    public UI Interfaz { get => UIEstatica; }
    public int Probability { get => CoinSpawnProbability[actualIndex]; }
    public int DamageInterval { get => DamageSysInterval[actualIndex]; }
    public int DamageTimer { get => DamageSysTimer[actualIndex]; }
    public PlayerProp GetPropertys { get => PlayerRound[actualIndex]; }
    public int Monedas { get; private set; }
    public bool OnShop { get; set; }
    //TODO:
    // IMPORANTE: Que a partir de estos valores el mapa y el jugador no dependa de nada mas.

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        actualIndex = 0;
        Monedas = actualIndex = 0;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerRound = new PlayerProp[mapIndexs.Length];
        for(int i = 0; i < PlayerRound.Length; ++i)
            PlayerRound[i] = new PlayerProp(2);
    }
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (timers[actualIndex] > 0f && !OnShop)
        {
            timers[actualIndex] -= Time.fixedDeltaTime;
            if (timers[actualIndex] <= 0f)
            {
                OnShop = true;
                SceneManager.LoadSceneAsync(mapIndexs[actualIndex]+1);
                actualIndex++;
                Player.GetComponent<PlayerMovement>().RealodPowerUps();
            }
        }
    }
    public void AddCoin()
    {
        Monedas++;
    }
}
