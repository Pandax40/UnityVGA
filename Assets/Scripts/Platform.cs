using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static int Probability;

    [SerializeField] private GameObject coinPrefab;
    private new Collider2D collider;
    private GameObject coinSpawned;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        timer = Random.Range(5, 6);
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
            if(random < Probability && coinSpawned == null)
            {
                coinSpawned = Instantiate(coinPrefab, transform.position + new Vector3(0,1.7f,0), Quaternion.identity);
                Destroy(coinSpawned, 8f);
            }
            timer = Random.Range(5, 6);
        }
    }
}
