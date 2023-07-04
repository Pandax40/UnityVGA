using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripter : MonoBehaviour
{
    [SerializeField] private GameObject FireSpawner;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(5, 20);
    }

    // Update is called once per frame
    void Update()
    {   
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                FireSpawner.GetComponent<SpawnFire>().timer = Random.Range(5, 10);
                timer = Random.Range(5, 20);
            }
        }
    }

    private void FixedUpdate()
    {

    }
}
