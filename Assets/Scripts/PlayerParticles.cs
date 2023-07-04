using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public PlayerMovement playerMovement;
    // Start is called before the first frame update

    // Update is called once per frame

    void Start()
    {
        
    }
    void Update()
    {
        if (playerMovement.canJump && playerMovement.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
