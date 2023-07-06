using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private SectionManager sectionManager;
    [SerializeField] private SpawnFire fireSpawner;
    public GameObject DamagePlayer;
    private bool death;
    public static float timer;
    // Start is called before the first frame update
    void Start()
    {
        death = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.RemoveHeart() && !death && timer <= 0f)
        {
            GameObject Sonido = Instantiate(DamagePlayer);
            Destroy(Sonido,1f);
            fireSpawner.PauseFire();
            collision.transform.position = sectionManager.GetSpawnPos();
            GameManager.Instance.Interfaz.ShakeHearts(1f);
            death = false;
            timer = 1f;
        }
        else 
            death = true;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }
}
