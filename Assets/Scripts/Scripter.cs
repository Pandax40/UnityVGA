using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripter : MonoBehaviour
{
    [SerializeField] private GameObject FireSpawner;
    public CameraShake CameraShake;
    private Vector3 OgCamerapos;
    public bool Spawning;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(5, 20);
        OgCamerapos = CameraShake.transform.position;
        Spawning = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                Spawning = true;
                FireSpawner.GetComponent<SpawnFire>().timer = Random.Range(5, 10);
                timer = Random.Range(5, 20);
            }
        }
        if (!Spawning) CameraShake.transform.position = OgCamerapos;
    }

    private void FixedUpdate()
    {

    }
}
