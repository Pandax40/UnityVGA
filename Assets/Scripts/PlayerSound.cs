using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource AudioSource;
    public PlayerMovement playerMovement;
    public float AudioRunTimer;
    private float timer;

    private void Start()
    {
        timer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerMovement.canJump && GameManager.Instance.Player.GetComponent<Rigidbody2D>().velocity.x != 0 && playerMovement.canSlide)
        {
            if (!AudioSource.isPlaying && timer <= 0)
            {
                AudioSource.pitch = (Random.Range(0.8f, 1.2f));
                AudioSource.Play();
                timer = AudioRunTimer;
            }
        }
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
    }
}
