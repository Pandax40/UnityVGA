using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static int Probability;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private float DestroyTimer;
    private new Collider2D collider;
    private GameObject Spawned;
    private float timer;            //Delay entre randoms
    private PhysicsMaterial2D materialSlime;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        timer = Random.Range(2f, 4f);
        materialSlime = new PhysicsMaterial2D();
        materialSlime.bounciness = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Player.transform.position.y <= transform.position.y + 1.675f)
            collider.enabled = false;
        else 
            collider.enabled = true;
        timer -= Time.deltaTime;
        if(timer < 0f)
        {   
            int random = Random.Range(0, 100);
            if(random < Probability && Spawned == null)
            {
                int randomHeart = Random.Range(0, 100);
                if (GameManager.Instance.GetPropertys.CoinsToHearts && randomHeart < 6)
                    Spawned = Instantiate(heartPrefab, transform.position + new Vector3(0,1.7f,0), Quaternion.identity);
                else
                    Spawned = Instantiate(coinPrefab, transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity);
                Destroy(Spawned, DestroyTimer);
            }
            timer = Random.Range(2f, 4f);
        }

        if(gameObject.tag == "Slime")
        {
            if (Mathf.Abs(GameManager.Instance.Player.GetComponent<Rigidbody2D>().velocity.y) < 10f)
                collider.sharedMaterial = new PhysicsMaterial2D();
            else
                collider.sharedMaterial = materialSlime;

        }
    }
}
