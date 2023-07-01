using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float HorizontalVelocty;
    [SerializeField] private float VerticalVelocty;
    [SerializeField] private Animator Animator;

    private new Rigidbody2D rigidbody;
    private bool canJump; //Si puede saltar.
    private bool jumping; //Si la Key esta pulsada.
    private LayerMask raycastLayer;
    private float raycastDistance;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        raycastLayer = LayerMask.GetMask("Raycast");
        raycastDistance = 1.7f;
    }


    void Update()
    {

    }

    private void FixedUpdate()
    {
        canJump = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, raycastLayer).collider != null;
        if (canJump == false) canJump = Physics2D.Raycast(transform.position - new Vector3(-1, 0, 0), Vector2.down, raycastDistance, raycastLayer).collider != null;
        if (canJump == false) canJump = Physics2D.Raycast(transform.position - new Vector3(1, 0, 0), Vector2.down, raycastDistance, raycastLayer).collider != null;
        
        Animator.SetBool("Jumping", !canJump);
        Animator.SetFloat("Jumping Velocity", Mathf.Clamp(rigidbody.velocity.y, -1f, 1f));
        Animator.SetFloat("Horizontal Speed", Mathf.Clamp(rigidbody.velocity.x, -1f, 1f));

        if (rigidbody.velocity.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (rigidbody.velocity.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);

        if(canJump && jumping)
            rigidbody.AddForce(Vector2.up * VerticalVelocty, ForceMode2D.Impulse);

        //Debug Info
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, -raycastDistance, 0));
        Debug.DrawLine(transform.position - new Vector3(-1, 0, 0), transform.position - new Vector3(-1, 0, 0) + new Vector3(0, -raycastDistance, 0));
        Debug.DrawLine(transform.position + new Vector3(-1, 0, 0), transform.position + new Vector3(-1, 0, 0) + new Vector3(0, -raycastDistance, 0));
    }

    public void HorizontalMove(InputAction.CallbackContext context)
    {
        //Establece la velocidad horizontal
        rigidbody.velocity = new Vector2(context.ReadValue<float>() * HorizontalVelocty, rigidbody.velocity.y);
    }

    public void VerticalMove(InputAction.CallbackContext context)
    {
        jumping = context.performed;
    }

}
