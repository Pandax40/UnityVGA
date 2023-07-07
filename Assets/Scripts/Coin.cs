using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject CoinPlayer;
    [SerializeField] private GameObject PickupParticles;
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
        else if (collision.tag == "Fire")
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
