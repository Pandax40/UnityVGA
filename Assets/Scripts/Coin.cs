using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinPlayer;
    public GameObject PickupParticles;
    private bool isTaked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTaked)
        {
            isTaked = true;
            GameManager.Instance.AddCoin();
            GameObject Audio = Instantiate(CoinPlayer);
            Destroy(Audio,1f);
            Destroy(gameObject, 0.1f);
            GameManager.Instance.Interfaz.UpdateCoins();
            Instantiate(PickupParticles, collision.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
