using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnFire : MonoBehaviour
{
    [SerializeField] private GameObject FireBall;
    [SerializeField] private float RangeX;              //rangos de x entre los que spawnea bolas
    [SerializeField] private float initialVelocity;
    [SerializeField] private float freq;
    [SerializeField] private float MaxSoundTimer;
    public float timer { get; set; }
    public CameraShake CameraShake;
    public Scripter Scripter;
    public AudioSource AudioSource;
    public AudioClip AudioClip;
    public GameObject ColumnPlayer;
    private GameObject SoundColumn;
    [SerializeField] private GameObject WarningColumn;

    private float auxFreq;
    private bool FirstSpawn;
    private Vector2 centerPosition;
    private GameObject Column;
    private float SoundTimer;

    // Start is called before the first frame update
    void Start()
    {
        auxFreq = freq;
        timer = 0;
        centerPosition = new Vector2(Random.Range(-12, 12), 20);
        FirstSpawn = true;
        SoundTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SoundTimer -= Time.deltaTime;
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                centerPosition = new Vector2(Random.Range(-12, 12), 20);
                FirstSpawn = true;
            }
        }
        if (FirstSpawn && timer > 0)
        {
            Column = Instantiate(WarningColumn, new Vector3(centerPosition.x, 0, 0f), Quaternion.identity);
            SoundColumn = Instantiate(ColumnPlayer);
            Destroy(Column, 5f); //Cambiar 5f por el intervalo del game manager
            FirstSpawn = false;
        }
    }

    private void FixedUpdate()
    {   
        if (timer > 0f && Column == null)
        {
            auxFreq -= Time.fixedDeltaTime;
            if (auxFreq <= 0f)
            {
                if (SoundTimer <= 0)
                {
                    AudioSource.PlayOneShot(AudioClip, 2f);
                    SoundTimer = MaxSoundTimer;
                }
                Destroy(SoundColumn);
                float randomx = Random.Range(-RangeX, RangeX);
                FireSpawn(new Vector3(randomx + centerPosition.x, centerPosition.y, 0f));
                auxFreq = freq;
            }
        }
        if (timer <= 0) Scripter.Spawning = false;
    }

    public void FireSpawn(Vector3 pos)
    {
        CameraShake.ShakeCamera();
        GameObject Bola = Instantiate(FireBall, pos, Quaternion.identity);
        Bola.transform.eulerAngles = new Vector3(0, 0, -90);
        Bola.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -initialVelocity);
        Destroy(Bola, 0.8f);
    }
}
