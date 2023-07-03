using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;

    private GameObject platform;
    private float distanceX;
    private float distanceY;

    //Debug
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        distanceX = this.GetComponent<BoxCollider2D>().size.x / 2;
        distanceY = this.GetComponent<BoxCollider2D>().size.y / 2;
        GeneratePlatform();
        //time = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0f)
        {
            time -= Time.deltaTime;
            if(time <= 0f)
            {
                time = 10f;
                GeneratePlatform();
            }
        }
    }

    public BoxCollider2D GeneratePlatform()
    {
        float randomPosX = Random.Range(-distanceX, distanceX) + transform.position.x;
        float randomPosY = Random.Range(-distanceY, distanceY) + transform.position.y;
        if (platform != null) 
            Destroy(platform);
        platform = Instantiate(platformPrefab, new Vector3(randomPosX, randomPosY, 0), Quaternion.identity, transform);
        return platform.GetComponent<BoxCollider2D>();
    }
}
