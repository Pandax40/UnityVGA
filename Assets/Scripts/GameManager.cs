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
    public int hearts { get; set; }
    public bool extraHeart { get; set; }
    public bool plusVelocity { get; set; }
    public bool plusJump { get; set; }
    public bool dobleJump { get; set; }
    public bool dobleWallJump { get; set; }
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
    [SerializeField] private GameObject Loading;
    [SerializeField] private GameObject ControlScreen;

    private int actualIndex;

    private AsyncOperation loadProgress;

    private bool firstLevel;
    public GameObject Player { get => PlayerScene; }
    public UI Interfaz { get => UIEstatica; }
    public int Probability { get => CoinSpawnProbability[actualIndex]; }
    public int DamageInterval { get => DamageSysInterval[actualIndex]; }
    public int DamageTimer { get => DamageSysTimer[actualIndex]; }
    public PlayerProp GetPropertys { get => PlayerRound[actualIndex]; }
    public int Monedas { get; private set; }
    public bool OnShop { get; set; }

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Monedas = actualIndex = 0;
        loadProgress = null;
        firstLevel = true;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadFirstLevel()
    {
        PlayerRound = new PlayerProp[mapIndexs.Length];
        for (int i = 0; i < PlayerRound.Length; ++i)
            PlayerRound[i] = new PlayerProp(3);
        ControlScreen.SetActive(true);
    }
    void Update()
    {
        if(loadProgress != null && loadProgress.isDone)
        {
            Loading.SetActive(false);
            Player.SetActive(true);
            Interfaz.gameObject.SetActive(true);
            loadProgress = null;
            firstLevel = false;
        }
        else if(loadProgress != null)
        {
            Loading.SetActive(true);
            //Interfaz.gameObject.SetActive(false);
            Player.SetActive(false);
        }
            
    }

    public void LoadScene(int mapIndex)
    {
        loadProgress = SceneManager.LoadSceneAsync(mapIndexs[mapIndex]);
    }

    private void FixedUpdate()
    {
        if (timers[actualIndex] > 0f && !OnShop && loadProgress == null && !firstLevel)
        {
            timers[actualIndex] -= Time.fixedDeltaTime;
            if (timers[actualIndex] <= 0f)
            {
                OnShop = true;
                SceneManager.LoadSceneAsync(mapIndexs[actualIndex]+1);
                actualIndex++;
                Player.GetComponent<PlayerMovement>().ReloadPlayer();
            }
        }
    }
    public void AddCoin()
    {
        Monedas++;
    }

    //Indica si se a ha muerto el personaje.
    public bool RemoveHeart()
    {
        for(int i = actualIndex; i < mapIndexs.Length; ++i)
        {
            --PlayerRound[i].hearts;
        }
        Interfaz.UpdateHearts();
        if (GetPropertys.hearts == 0)
        {
            Loading.SetActive(true);
            return true;
        }
        return false;
    }

    public void AddHeart()
    {
        Monedas++;
    }
}
