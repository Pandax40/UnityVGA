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


    private float auxFreq;
    private Vector2 centerPosition;

    // Start is called before the first frame update
    void Start()
    {
        auxFreq = freq;
        timer = 0;
        float randomx = Random.Range(-12, 12);
        centerPosition = new Vector2(randomx, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0f)
            timer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (timer > 0f && auxFreq > 0f)
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
        GameObject Bola = Instantiate(FireBall, pos, Quaternion.identity);
        Bola.transform.eulerAngles = new Vector3(0, 0, -90);
        Bola.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -initialVelocity);
        Destroy(Bola, 0.8f);
    }
}
