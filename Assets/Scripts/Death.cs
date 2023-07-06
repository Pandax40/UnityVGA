using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private SectionManager sectionManager;
    public GameObject DamagePlayer;
    private bool death;
    // Start is called before the first frame update
    void Start()
    {
        death = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.RemoveHeart() && !death)
        {
            GameObject Sonido = Instantiate(DamagePlayer);
            Destroy(Sonido,1f);
            collision.transform.position = sectionManager.GetSpawnPos();
            GameManager.Instance.Interfaz.ShakeHearts(1f);
            death = false;
        }
        else 
            death = true;
    }
}
