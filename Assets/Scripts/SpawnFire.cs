using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnFire : MonoBehaviour
{
    public GameObject FireBall;
    private float auxiliartimer;
    [SerializeField] private float RangoX; //rangos de x entre los que spawnea bolas
    [SerializeField] private float RangoMenosX;
    [SerializeField] private float maxauxiliartimer; //cooldown de bolas
    public FireScripter FireScripter;

    // Start is called before the first frame update
    void Start()
    {
        auxiliartimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (auxiliartimer <= 0 && FireScripter.CanSpawn)
        {
            float randomx = Random.Range(RangoMenosX, RangoX);
            Vector3 vec = new Vector3(randomx, 0, 0);
            FireSpawn(vec);
            auxiliartimer = maxauxiliartimer;
        }
    }

    private void FixedUpdate()
    {
        auxiliartimer -= Time.fixedDeltaTime;
    }

    public void FireSpawn(Vector3 pos)
    {
        GameObject Bola = Instantiate(FireBall, transform.position + pos, Quaternion.identity);
        Bola.transform.eulerAngles = new Vector3(0, 0, 270);
        Destroy(Bola, 1f);
    }
}
