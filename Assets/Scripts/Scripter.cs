using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripter : MonoBehaviour
{
    [SerializeField] private GameObject FireSpawner;
    public CameraShake CameraShake;
    private Vector3 OgCamerapos;
    public static bool Spawning;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        int damageInterval = GameManager.Instance.DamageInterval;
        timer = Random.Range(damageInterval, damageInterval + 1f);
        OgCamerapos = CameraShake.transform.position;
        Spawning = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (timer > 0 && !Spawning)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                Spawning = true;
                FireSpawner.GetComponent<SpawnFire>().timer = Random.Range(2f, 3f);
                int damageInterval = GameManager.Instance.DamageInterval;
                timer = Random.Range(damageInterval, damageInterval + 1f);
            }
        }
        if (!Spawning) 
            CameraShake.transform.position = OgCamerapos;
    }

    private void FixedUpdate()
    {

    }
}
