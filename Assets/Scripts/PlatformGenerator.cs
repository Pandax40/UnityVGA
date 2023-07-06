using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private GameObject platform;
    private float distanceX;
    private float distanceY;
    private int i;
    private int j;
    private float timer;
    private bool isDestroyed;

    void Start()
    {
        distanceX = this.GetComponent<BoxCollider2D>().size.x / 2;
        distanceY = this.GetComponent<BoxCollider2D>().size.y / 2;
    }

    public Transform GeneratePlatform(GameObject platformPrefab, int i, int j)
    {
        this.i = i;
        this.j = j;
        float randomPosX = Random.Range(-distanceX, distanceX) + transform.position.x;
        float randomPosY = Random.Range(-distanceY, distanceY) + transform.position.y;
        if (platform != null) 
            Destroy(platform);
        platform = Instantiate(platformPrefab, new Vector3(randomPosX, randomPosY, 0), Quaternion.identity, transform);
        return platform.transform;
    }

    private void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                transform.parent.parent.GetComponent<SectionManager>().ReloadPlatform(i,j);
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Fire") && !isDestroyed)
        {
            isDestroyed = true;
            timer = 2f;
        }
        else if(isDestroyed && !collision.gameObject.CompareTag("Fire") && !collision.gameObject.CompareTag("Player"))
        {
            isDestroyed = false;
        }
            
    }
}
