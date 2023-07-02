using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float HorizontalVelocty;
    [SerializeField] private float VerticalVelocty;
    [SerializeField] private float SlideForce;
    [SerializeField] private float SlideDrag;
    [SerializeField] private Animator Animator;
    [SerializeField] private Collider2D[] animationColiders; //Orden: [0]Idle/Jump [1]Run/AirDash [2]Slide/Shift


    private new Rigidbody2D rigidbody;
    private bool canJump; //Si puede saltar.
    private bool jumping; //Si la Key esta pulsada.
    private bool canSlide;
    private bool canMove;
    private LayerMask raycastLayer;
    private float raycastGroundDistance;
    private float raycastHorizontalDistance;
    private float speed;
    private int actIndex; //Indice del colider actual

    //TODO:
    // - Implementar el shifteo
    // - Implementar el wall jump.
    // - Ajustar el personaje al mapa [Primero tener mapa]
    // - Raycast a los lados para evitar el bug de las animaciones de correr pegado a la pared

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        raycastLayer = LayerMask.GetMask("Raycast");
        raycastGroundDistance = 1.7f; //Mejor distancia al suelo actual
        raycastHorizontalDistance = 1.2f;

        canSlide = true;
        canMove = true;
        jumping = false;
        actIndex = 0;
    }


    void Update()
    {
        //Redondear la velocidad [BUG ANIMACIONES]
        string roundVelociadX = rigidbody.velocity.x.ToString("F2");
        float fRoundVelociadX = float.Parse(roundVelociadX);
        string roundVelociadY = rigidbody.velocity.y.ToString("F2");
        float fRoundVelociadY = float.Parse(roundVelociadY);
        rigidbody.velocity = new Vector2(fRoundVelociadX, fRoundVelociadY);
    }

    private void FixedUpdate()
    {
        //Variables modificables
        float direction = Mathf.Clamp(transform.eulerAngles.y - 90, -1f, 1f); //Direccion del personaje [IZQ](-1f) o [DER](1f)
        float distanceRaycasts = 0.55f; //Distancia entre Raycast.

        //Detectar si se pude saltar
        Vector3 raycastCenter = transform.position + new Vector3(-0.233f, 0, 0) * -direction;
        canJump = Physics2D.Raycast(raycastCenter, Vector2.down, raycastGroundDistance, raycastLayer).collider != null;
        if (canJump == false) canJump = Physics2D.Raycast(raycastCenter + new Vector3(-distanceRaycasts, 0, 0), Vector2.down, raycastGroundDistance, raycastLayer).collider != null;
        if (canJump == false) canJump = Physics2D.Raycast(raycastCenter + new Vector3(distanceRaycasts, 0, 0), Vector2.down, raycastGroundDistance, raycastLayer).collider != null;

        //Detectar si hay una pared
        canMove = Physics2D.Raycast(transform.position, Vector2.right, raycastHorizontalDistance, raycastLayer).collider == null;

        //Slide restrictivo
        if (!canSlide && Mathf.Abs(rigidbody.velocity.x) < HorizontalVelocty)
        {
            canSlide = true;
            rigidbody.drag = 0f;
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
        }

        //Actualizar animaciones
        Animator.SetBool("Jumping", !canJump);
        Animator.SetBool("Slide", !canSlide);
        Animator.SetFloat("Jumping Velocity", Mathf.Clamp(rigidbody.velocity.y, -1f, 1f));
        Animator.SetFloat("Horizontal Speed", Mathf.Clamp(rigidbody.velocity.x, -1f, 1f));

        //Actualizar colider
        if (Mathf.Clamp(rigidbody.velocity.x, -1f, 1f) != 0f)
        {
            if(canJump && !canSlide) 
                ColiderActivator(2);
            else 
                ColiderActivator(1);
        }
        else ColiderActivator(0);

        //Rotacion objeto
        if (rigidbody.velocity.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (rigidbody.velocity.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);

        //Jumping continuo
        if (canJump && jumping && canSlide)
            rigidbody.AddForce(Vector2.up * VerticalVelocty, ForceMode2D.Impulse);

        //Debug Info
        Debug.DrawLine(transform.position, transform.position + Vector3.right * raycastHorizontalDistance);
        Debug.DrawLine(raycastCenter, raycastCenter + Vector3.down * raycastGroundDistance); 
        Debug.DrawLine(raycastCenter + new Vector3(-distanceRaycasts, 0, 0), raycastCenter + new Vector3(-distanceRaycasts, 0, 0) + new Vector3(0, -raycastGroundDistance, 0));
        Debug.DrawLine(raycastCenter + new Vector3(distanceRaycasts, 0, 0), raycastCenter + new Vector3(distanceRaycasts, 0, 0) + new Vector3(0, -raycastGroundDistance, 0));
    }

    private void ColiderActivator(int index)
    {
        animationColiders[actIndex].enabled = false;
        animationColiders[index].enabled = true;
        actIndex = index;
    }

    public void HorizontalMove(InputAction.CallbackContext context)
    {
        //Establece la velocidad horizontal
        speed = context.ReadValue<float>() * HorizontalVelocty;
        if (canSlide && canMove) rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    }

    public void VerticalMove(InputAction.CallbackContext context)
    {
        jumping = context.performed;
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if(context.performed && canSlide && rigidbody.velocity.x != 0f)
        {
            canSlide = false;

            //Drag para reducir velocidad
            rigidbody.drag = SlideDrag;

            //Fuerza en la direccion actual
            float direction = Mathf.Round(Mathf.Clamp(rigidbody.velocity.x, -1f, 1f));
            rigidbody.AddForce(new Vector2(SlideForce * direction, 0f), ForceMode2D.Impulse);
        }
    }

}
