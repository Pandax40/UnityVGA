using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PlayerProp
{
    public PlayerProp(int hearts) 
    {
        runParticles = 0;
        plusJump = plusVelocity = dobleWallJump = dobleJump = extraHeart = false;
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

    public static Vector3 SpawnPos;

    [SerializeField] private PlayerProp[] PlayerRound;
    [SerializeField] private int[] CoinSpawnProbability;
    [SerializeField] private int[] DamageSysInterval;
    [SerializeField] private int[] DamageSysTimer;
    [SerializeField] private int[] mapIndexs;
    [SerializeField] private float[] timers;
    [SerializeField] private GameObject PlayerScene;
    [SerializeField] private GameObject UIEstatica;
    [SerializeField] private GameObject Loading;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject PauseMenu;

    private int actualIndex;

    private AsyncOperation loadProgress;
    public GameObject Player { get => PlayerScene; }
    public UI Interfaz { get => UIEstatica.GetComponent<UI>(); }
    public GameObject GameOver { get => GameOverScreen; }
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
            Destroy(PlayerScene);
            Destroy(Loading);
            Destroy(GameOverScreen);
            Destroy(PauseMenu);
            Destroy(UIEstatica);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Monedas = actualIndex = 0;
        loadProgress = null;
        DontDestroyOnLoad(PlayerScene);
        DontDestroyOnLoad(Loading);
        DontDestroyOnLoad(GameOverScreen);
        DontDestroyOnLoad(PauseMenu);
        DontDestroyOnLoad(UIEstatica);
        DontDestroyOnLoad(gameObject);
        PlayerScene.SetActive(false);
        Loading.SetActive(false);
        GameOverScreen.SetActive(false);
        PauseMenu.SetActive(false);
        UIEstatica.SetActive(false);
    }

    private void ReloadAll()
    {
        PlayerRound = new PlayerProp[mapIndexs.Length];
        for (int i = 0; i < PlayerRound.Length; ++i)
            PlayerRound[i] = new PlayerProp(3);
        actualIndex = 0;
    }

    public void LoadFirstLevel()
    {
        ReloadAll();
        loadProgress = SceneManager.LoadSceneAsync(mapIndexs[0]);
    }

    public void LoadMainMenu()
    {
        ReloadAll();
        loadProgress = SceneManager.LoadSceneAsync(0);
    }
    void Update()
    {
        if(loadProgress != null && loadProgress.isDone)
        {
            Loading.SetActive(false);
            PauseMenu.SetActive(false);
            GameOver.SetActive(false);
            loadProgress = null;
            if (SceneManager.GetActiveScene().buildIndex > 1)
            {
                Interfaz.gameObject.SetActive(true);
                Player.transform.position = SpawnPos;
                Player.SetActive(true);
                if(OnShop) Player.transform.position = new Vector3(-6,-2.6f,0);
            }
            else
            {
                Player.SetActive(false);
                Interfaz.gameObject.SetActive(false);
            }
        }
        else if(loadProgress != null)
        {
            Loading.SetActive(true);
            Interfaz.gameObject.SetActive(false);
            Player.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (timers[actualIndex] > 0f && !OnShop && loadProgress == null)
        {
            timers[actualIndex] -= Time.fixedDeltaTime;
            if (timers[actualIndex] <= 0f)
            {
                OnShop = true;
                actualIndex++;
                Player.GetComponent<PlayerMovement>().ReloadPlayer();
                loadProgress = SceneManager.LoadSceneAsync(mapIndexs[actualIndex-1]+1);
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
