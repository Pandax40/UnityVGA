using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScripter : MonoBehaviour
{
    [SerializeField] private float maxTimer;
    [SerializeField] private float RangoX;
    [SerializeField] private float RangoMenosX;
    [SerializeField] private float maxAuxTimer;
    private float AuxTimer;
    public bool CanSpawn;
    public GameObject FireSpawner;
    private float timer;
    public SpawnFire SpawnFire;
    // Start is called before the first frame update
    void Start()
    {
        timer = maxTimer;
        CanSpawn = false;
        AuxTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timer);
        if (timer <= 0)
        {
            FireSpawner.transform.position = new Vector3(0, 20, 0);
            float randomx = Random.Range(RangoMenosX, RangoX);
            Vector3 vec = new Vector3(randomx, 0, 0);
            FireSpawner.transform.position += vec;
            timer = maxTimer;
            CanSpawn = true;
            AuxTimer = maxAuxTimer;
        }
        if (AuxTimer <= 0) CanSpawn = false;
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        AuxTimer -= Time.fixedDeltaTime;
    }
}
