using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private GameObject HeartPlayer;
    [SerializeField] private GameObject PickupParticles;
    private bool isTaked = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isTaked)
        {
            isTaked = true;
            GameManager.Instance.AddHearts(1);
            GameManager.Instance.Interfaz.UpdateHearts();
            GameObject Audio = Instantiate(HeartPlayer);
            Destroy(Audio, 1f);
            Destroy(gameObject, 0.1f);
            Instantiate(PickupParticles, collision.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
