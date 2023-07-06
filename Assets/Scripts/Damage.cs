using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject DamagePlayer;
    private float health;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        health = 2f;
    }

    private void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                health = 2f;
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Fire") && Death.timer <= 0f)
        {
            health -= 0.1f;
            if(health <= 0f)
            {
                GameObject Sondio = Instantiate(DamagePlayer);
                Destroy(Sondio, 1f);
                GameManager.Instance.RemoveHeart();
                GameManager.Instance.Interfaz.ShakeHearts(1f);
                health = 9f;
                timer = 3f;
            }
        }
    }
}
