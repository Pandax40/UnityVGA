using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float HorizontalVelocty;
    public float VerticalVelocty;

    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HorizontalMove(InputAction.CallbackContext ctx)
    {
        //Establece la velocidad horizontal
        rigidbody.velocity = new Vector2(ctx.ReadValue<float>() * HorizontalVelocty, rigidbody.velocity.y);
    }

    public void VerticalMove(InputAction.CallbackContext ctx)
    {
        rigidbody.AddForce(Vector2.up * VerticalVelocty, ForceMode2D.Impulse);
    }

}
