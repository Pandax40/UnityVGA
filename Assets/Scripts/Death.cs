using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private SectionManager sectionManager;
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
            collision.transform.position = sectionManager.GetSpawnPos();
            death = false;
        }
        else 
            death = true;
    }
}
