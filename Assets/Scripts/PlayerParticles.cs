using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject FallParticles;
    private bool FallDetection;
    // Start is called before the first frame update

    // Update is called once per frame

    void Start()
    {
        FallDetection = false;
    }
    void FixedUpdate()
    {
        if (playerMovement.canJump && playerMovement.GetComponent<Rigidbody2D>().velocity.x != 0 && playerMovement.canSlide)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (!playerMovement.canJump)
        {
            FallDetection = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && FallDetection) //ground
        {
            GameObject ParticulaCaida = Instantiate(FallParticles, transform.GetChild(1));
            Destroy(ParticulaCaida, 2f);
            FallDetection = false;
        }
    }
}
