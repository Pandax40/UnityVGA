using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public struct PlayerProp
{
    public PlayerProp(int hearts) 
    {
        runParticles = 0;
        plusJump = plusVelocity = dobleWallJump = dobleJump = extraHeart = CoinsToHearts = DoubleCoins = false;
        this.hearts = hearts; 
    }
    public int runParticles; //Maps [0] Forest [1] Cave [2] Castle
    public int hearts { get; set; }
    public bool extraHeart { get; set; }
    public bool plusVelocity { get; set; }
    public bool plusJump { get; set; }
    public bool dobleJump { get; set; }
    public bool dobleWallJump { get; set; }
    public bool CoinsToHearts { get; set; }
    public bool DoubleCoins { get; set; }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Vector3 SpawnPos;

    [SerializeField] private PlayerProp[] PlayerRound;
    [SerializeField] private int[] CoinSpawnProbability;
    [SerializeField] private int[] DamageSysInterval;
    [SerializeField] private int[] DamageSysWarn;
    [SerializeField] private int[] mapIndexs;
    [SerializeField] private float[] timers;
    [SerializeField] private GameObject PlayerScene;
    [SerializeField] private GameObject UIEstatica;
    [SerializeField] private GameObject Loading;
    [SerializeField] private GameObject PauseMenu;

    private int actualIndex;
    private float auxTimer;

    private AsyncOperation loadProgress;
    public GameObject Player { get => PlayerScene; }
    public UI Interfaz { get => UIEstatica.GetComponent<UI>(); }
    public GameObject Pause { get => PauseMenu; }
    public GameObject LoadingScreen { get => Loading; }
    public int Probability { get => CoinSpawnProbability[actualIndex]; }
    public int DamageInterval { get => DamageSysInterval[actualIndex]; }
    public int DamageWarn { get => DamageSysWarn[actualIndex]; }
    public PlayerProp GetPropertys { get => PlayerRound[actualIndex]; }
    public int Monedas { get; private set; }
    public bool OnShop { get; private set; }
    public bool BuyPerformed { get; private set; }
    public bool Inmortal { get; private set; }
    public bool[] BuyBuffer { get; private set; }

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(PlayerScene);
            Destroy(Loading);
            Destroy(PauseMenu);
            Destroy(UIEstatica);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Monedas = actualIndex = 0;
        loadProgress = null;
        BuyPerformed = false;
        Inmortal = false;
        BuyBuffer = new bool[5];
        DontDestroyOnLoad(PlayerScene);
        DontDestroyOnLoad(Loading);
        DontDestroyOnLoad(PauseMenu);
        DontDestroyOnLoad(UIEstatica);
        DontDestroyOnLoad(gameObject);
        PlayerScene.SetActive(false);
        Loading.SetActive(false);
        PauseMenu.SetActive(false);
        UIEstatica.SetActive(false);
    }

    private void ReloadAll()
    {
        PlayerRound = new PlayerProp[mapIndexs.Length];
        for (int i = 0; i < PlayerRound.Length; ++i)
            PlayerRound[i] = new PlayerProp(3);
        actualIndex = 0;
        Monedas = 0;
    }

    public void LoadFirstLevel()
    {
        ReloadAll();
        loadProgress = SceneManager.LoadSceneAsync(mapIndexs[0]);
    }

    public void LoadCredits()
    {
        loadProgress = SceneManager.LoadSceneAsync(1);
    }

    public void LoadMainMenu()
    {
        ReloadAll();
        Interfaz.UpdateHearts();
        Interfaz.UpdateCoins();
        loadProgress = SceneManager.LoadSceneAsync(0);
    }
    void Update()
    {
        if(loadProgress != null && loadProgress.isDone)
        {
            Loading.SetActive(false);
            PauseMenu.SetActive(false);
            loadProgress = null;
            auxTimer = timers[actualIndex];
            if (SceneManager.GetActiveScene().buildIndex > 2)
            {
                Interfaz.gameObject.SetActive(true);
                Player.SetActive(true);
                Player.transform.position = SpawnPos;
                if (OnShop)
                {
                    Player.transform.position = new Vector3(-6,-2.6f,0);
                    Interfaz.transform.GetChild(3).gameObject.SetActive(false);
                    PoUpManager.IsUpdated = true;
                }
                else
                    Interfaz.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                Player.SetActive(false);
                Interfaz.gameObject.SetActive(false);
                Interfaz.transform.GetChild(3).gameObject.SetActive(false);
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
        if (timers[actualIndex] > 0f && !OnShop && SceneManager.GetActiveScene().buildIndex > 2 && loadProgress == null)
        {
            timers[actualIndex] -= Time.fixedDeltaTime;
            Timer.timer = timers[actualIndex];
            if (timers[actualIndex] <= 0f)
            {
                OnShop = true;
                timers[actualIndex] = auxTimer;
                actualIndex++;
                if(actualIndex < PlayerRound.Length)
                {
                    Player.GetComponent<PlayerMovement>().ReloadPlayer();
                    Interfaz.UpdateHearts();
                    loadProgress = SceneManager.LoadSceneAsync(mapIndexs[actualIndex-1]+1);
                }
                else
                {
                    Player.SetActive(false);
                    Interfaz.gameObject.SetActive(false);
                    timers[actualIndex - 1] = auxTimer;
                    actualIndex = 0;
                    OnShop = false;
                    loadProgress = SceneManager.LoadSceneAsync(1);
                }
            }
        }
    }
    public void AddCoin()
    {
        Monedas += PlayerRound[actualIndex].DoubleCoins ? 2 : 1;
    }

    //Indica si se a ha muerto el personaje.
    public bool RemoveHeart()
    {
        if(Inmortal) return false;
        for(int i = actualIndex; i < mapIndexs.Length; ++i)
        {
            --PlayerRound[i].hearts;
        }
        Interfaz.UpdateHearts();
        if (GetPropertys.hearts == 0)
        {
            Player.SetActive(false);
            Interfaz.gameObject.SetActive(false);
            timers[actualIndex] = auxTimer;
            loadProgress = SceneManager.LoadSceneAsync(2);
            return true;
        }
        return false;
        
    }

    public void BuyKey(InputAction.CallbackContext context)
    {
        BuyPerformed = context.performed;
    }

    public void InmortalKey(InputAction.CallbackContext context)
    {
        if(context.performed)
            Inmortal = !Inmortal;
    }

    public void BuyItem(int id)
    {
        int price = 0;
        switch(id)
        {
            case 0:
                price = 15;
                PlayerRound[actualIndex].CoinsToHearts = true;
                break;
            case 1:
                price = 15;
                PlayerRound[actualIndex].DoubleCoins = true;
                break;
            case 2:
                price = 15;
                AddHearts(1);
                break;
            case 3:
                price = 25;
                PlayerRound[actualIndex].extraHeart = true;
                AddHearts(1);
                break;
            case 4:
                price = 25;
                AddHearts(3);
                break;
            case 5:
                price = 25;
                PlayerRound[actualIndex].plusJump = true;
                break;
            case 6:
                price = 25;
                PlayerRound[actualIndex].plusVelocity = true;
                break;
            case 7:
                price = 50;
                for (int i = actualIndex; i < PlayerRound.Length; ++i)
                    PlayerRound[i].dobleJump = true;
                BuyBuffer[0] = true;
                break;
            case 8:
                price = 50;
                for (int i = actualIndex; i < PlayerRound.Length; ++i)
                    PlayerRound[i].dobleWallJump = true;
                BuyBuffer[1] = true;
                break;
            case 9:
                price = 50;
                for (int i = actualIndex; i < PlayerRound.Length; ++i)
                    PlayerRound[i].extraHeart = true;
                BuyBuffer[2] = true;
                AddHearts(1);
                break;
            case 10:
                price = 50;
                for (int i = actualIndex; i < PlayerRound.Length; ++i)
                    PlayerRound[i].plusJump = true;
                BuyBuffer[3] = true;
                break;
            case 11:
                price = 50;
                for (int i = actualIndex; i < PlayerRound.Length; ++i)
                    PlayerRound[i].plusVelocity = true;
                BuyBuffer[4] = true;
                break;
            default:
                break;
        }
        Monedas -= price;
        Interfaz.UpdateCoins();
        Interfaz.UpdateHearts();
        Player.GetComponent<PlayerMovement>().ReloadPlayer();
    }

    public void AddHearts(int heartsNum)
    {
        for(int i = actualIndex; i < PlayerRound.Length; ++i)
        {
            for (int j = heartsNum; j > 0; --j)
            {
                if (PlayerRound[i].hearts < 3) 
                    ++PlayerRound[i].hearts;
                else if(PlayerRound[i].hearts < 4 && PlayerRound[i].extraHeart) 
                    ++PlayerRound[i].hearts;
            }
        }
    }

    public void ShopStop()
    {
        OnShop = false;
        loadProgress = loadProgress = SceneManager.LoadSceneAsync(mapIndexs[actualIndex]);
    }
}
