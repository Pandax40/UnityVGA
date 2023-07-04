using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private GameObject platform;
    private float distanceX;
    private float distanceY;

    //Debug
    private float time;

    void Start()
    {
        distanceX = this.GetComponent<BoxCollider2D>().size.x / 2;
        distanceY = this.GetComponent<BoxCollider2D>().size.y / 2;
        //time = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(time > 0f)
        {
            time -= Time.deltaTime;
            if(time <= 0f)
            {
                time = 10f;
                GeneratePlatform();
            }
        }*/
    }

    public Transform GeneratePlatform(GameObject platformPrefab)
    {
        float randomPosX = Random.Range(-distanceX, distanceX) + transform.position.x;
        float randomPosY = Random.Range(-distanceY, distanceY) + transform.position.y;
        if (platform != null) 
            Destroy(platform);
        platform = Instantiate(platformPrefab, new Vector3(randomPosX, randomPosY, 0), Quaternion.identity, transform);
        return platform.transform;
    }
}
