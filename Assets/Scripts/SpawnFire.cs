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
    public float timer { get; set; }
    public CameraShake CameraShake;
    [SerializeField] private GameObject WarningColumn;

    private float auxFreq;
    private bool FirstSpawn;
    private Vector2 centerPosition;
    private GameObject Column;

    // Start is called before the first frame update
    void Start()
    {
        auxFreq = freq;
        timer = 0;
        centerPosition = new Vector2(Random.Range(-12, 12), 20);
        FirstSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
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
            Destroy(Column, GameManager.Instance.DamageInterval/10f);
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
                float randomx = Random.Range(-RangeX, RangeX);
                FireSpawn(new Vector3(randomx + centerPosition.x, centerPosition.y, 0f));
                auxFreq = freq;
            }
        }
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
