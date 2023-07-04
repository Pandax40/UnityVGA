using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isTaked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTaked)
        {
            isTaked = true;
            GameManager.Instance.AddCoin();
            Destroy(gameObject, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
