using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFire : MonoBehaviour
{
    public GameObject FireBall;
    private float timer;
    private float auxiliartimer;
    [SerializeField] private float RangoX;
    [SerializeField] private float RangoMenosX;

    // Start is called before the first frame update
    void Start()
    {
        timer = 100f;
        auxiliartimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (auxiliartimer <= 0)
        {
            float randomx = Random.Range(RangoMenosX, RangoX);
            Vector3 vec = new Vector3(randomx, 0, 0);
            FireSpawn(vec);
            auxiliartimer = 0.5f;
        }
    }

    private void FixedUpdate()
    {
        --timer;
        if (timer <= 0) Destroy(this);
        --auxiliartimer;
    }

    public void FireSpawn(Vector3 pos)
    {
        GameObject Bola = Instantiate(FireBall, transform.position + pos, Quaternion.identity);
        Bola.transform.eulerAngles = new Vector3(0, 0, 270);
        Destroy(Bola, 1f);
    }
}
