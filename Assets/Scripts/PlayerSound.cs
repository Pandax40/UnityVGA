using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource AudioSource;
    public PlayerMovement playerMovement;

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (playerMovement.canJump && GameManager.Instance.Player.GetComponent<Rigidbody2D>().velocity.x != 0 && playerMovement.canSlide)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.Play();
            }
        }
        else AudioSource.Stop();
    }
}
