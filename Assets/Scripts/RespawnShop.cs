using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnShop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = new Vector3(-6, -2.6f, 0);
        }
    }
}
