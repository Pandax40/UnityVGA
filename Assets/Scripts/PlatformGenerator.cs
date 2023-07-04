using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private GameObject platform;
    private float distanceX;
    private float distanceY;

    void Start()
    {
        distanceX = this.GetComponent<BoxCollider2D>().size.x / 2;
        distanceY = this.GetComponent<BoxCollider2D>().size.y / 2;
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
