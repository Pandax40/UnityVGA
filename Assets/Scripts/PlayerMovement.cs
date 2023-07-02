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
    [SerializeField] private float WallJumpForce;
    [SerializeField] private Animator Animator;
    [SerializeField] private Collider2D[] animationColiders; //Orden: [0]Idle/Jump [1]Run/AirDash [2]Slide/Shift

    private new Rigidbody2D rigidbody;
    private LayerMask raycastLayer;

    private bool canJump;    //Si puede saltar.
    private bool wasJumping; //Si estaba saltado. [Coliders]
    private bool wantJump;   //Si la Key esta pulsada. [Salto Continuo]
    private bool hasJumped; //Si estaba saltado. [Coliders]
    private bool canSlide;
    private bool canMoveForward;    //Detecta si hay pared
    private bool firstWallJump;     //Evita el salto repetido en la pared
    private bool firstSlide;        //Evita el doble slide de pared

    private float raycastGroundDistance;
    private float raycastHorizontalDistance;
    private float speed;        //Velocidad en teoria
    private float direction;    //Direccion actual
    private float auxDirection; //Evita bugs
    private float timerWallJump;
    private float timerWallStay;

    private int actIndex;       //Indice del colider actual

    //TODO:
    // - Implementar el wall jump.
    // - Ajustar el personaje al mapa [Primero tener mapa]
    // - Arreglar hitbox y centro de run
    // - Animacion fall

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        raycastLayer = LayerMask.GetMask("Raycast");
        raycastGroundDistance = 1.7f;       //Mejor distancia al suelo
        raycastHorizontalDistance = 0.9f;  //Mejor distancia a la pared

        canSlide = true;
        canMoveForward = true;
        wantJump = false;
        hasJumped = false;
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

        direction = Mathf.Round(Mathf.Clamp(transform.eulerAngles.y - 90, -1f, 1f) * -1f); //Direccion del personaje [IZQ](-1f) o [DER](1f)
        
        //Variables locales
        float distanceRaycasts = 0.55f; //Distancia entre Raycast.

        //Detectar si se pude saltar
        Vector3 raycastCenter = transform.position + new Vector3(-0.233f, 0, 0) * direction;
        canJump = Physics2D.Raycast(raycastCenter, Vector2.down, raycastGroundDistance, raycastLayer).collider != null;
        if (canJump == false) canJump = Physics2D.Raycast(raycastCenter + new Vector3(-distanceRaycasts, 0, 0), Vector2.down, raycastGroundDistance, raycastLayer).collider != null;
        if (canJump == false) canJump = Physics2D.Raycast(raycastCenter + new Vector3(distanceRaycasts, 0, 0), Vector2.down, raycastGroundDistance, raycastLayer).collider != null;

        //Detectar si hay una pared
        canMoveForward = Physics2D.Raycast(transform.position, Vector2.right * direction, raycastHorizontalDistance, raycastLayer).collider == null;
        
        //Actualizar animaciones
        Animator.SetBool("Jumping", !canJump);
        Animator.SetBool("Slide", !canSlide);
        Animator.SetFloat("Jumping Velocity", Mathf.Clamp(rigidbody.velocity.y, -1f, 1f));
        if(canMoveForward || (!canMoveForward && auxDirection != direction))
            Animator.SetFloat("Horizontal Speed", Mathf.Clamp(rigidbody.velocity.x, -1f, 1f));
        else
            Animator.SetFloat("Horizontal Speed", 0f);

        //Timer
        if (timerWallJump > 0f)
        {
            timerWallJump -= Time.deltaTime;
            if (timerWallJump < 0f) timerWallJump = 0f;
        }

        //Debug Info
        Debug.DrawLine(transform.position, transform.position + Vector3.right * raycastHorizontalDistance * direction);
        Debug.DrawLine(raycastCenter, raycastCenter + Vector3.down * raycastGroundDistance);
        Debug.DrawLine(raycastCenter + new Vector3(-distanceRaycasts, 0, 0), raycastCenter + new Vector3(-distanceRaycasts, 0, 0) + new Vector3(0, -raycastGroundDistance, 0));
        Debug.DrawLine(raycastCenter + new Vector3(distanceRaycasts, 0, 0), raycastCenter + new Vector3(distanceRaycasts, 0, 0) + new Vector3(0, -raycastGroundDistance, 0));
    }

    private void FixedUpdate()
    {
        //Move
        if (canSlide && firstWallJump)
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

        //Slide restrictivo
        if (!canSlide && Mathf.Abs(rigidbody.velocity.x) < HorizontalVelocty)
        {
            canSlide = true;
            rigidbody.drag = 0f;
        }
        if (!firstSlide) 
            firstSlide = canJump;

        //Actualizar colider
        if (Mathf.Clamp(rigidbody.velocity.x, -1f, 1f) != 0f)
        {
            if (!wasJumping && !canSlide)
                ColiderActivator(2);
            else if ((canJump && canSlide || !canSlide && !canJump) && canMoveForward)
                ColiderActivator(1);
            else
                ColiderActivator(0);
        }
        else ColiderActivator(0);

        //Rotacion objeto
        if (rigidbody.velocity.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (rigidbody.velocity.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);

        //Jumping continuo y WallJump
        if (canJump && wantJump && canSlide) //Salto normal
        {
            rigidbody.AddForce(Vector2.up * VerticalVelocty, ForceMode2D.Impulse);
            hasJumped = true;
            timerWallJump = 0.3f;
        }
        else if (!canJump && !canMoveForward && wantJump && firstWallJump && timerWallJump == 0f) //WallJump
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(new Vector2(-0.4f * direction, 1f) * WallJumpForce, ForceMode2D.Impulse);
            firstWallJump = false;
        }

        if (timerWallJump == 0f && !canMoveForward && hasJumped && firstWallJump)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Clamp(rigidbody.velocity.y, -3f, float.MaxValue));
        
        if (!firstWallJump)
            firstWallJump = canJump;
            
        if (hasJumped && timerWallJump == 0f) 
            hasJumped = !canJump;
    }

    private void ColiderActivator(int index)
    {
        animationColiders[actIndex].enabled = false;
        animationColiders[index].enabled = true;
        actIndex = index;
    }

    public void HorizontalMove(InputAction.CallbackContext context)
    {
        speed = context.ReadValue<float>() * HorizontalVelocty;
        auxDirection = direction;
    }

    public void VerticalMove(InputAction.CallbackContext context)
    {
        wantJump = context.performed;
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if(context.performed && canSlide && rigidbody.velocity.x != 0f && firstSlide)
        {
            canSlide = false;
            firstSlide = false;
            wasJumping = !canJump;

            //Drag para reducir velocidad
            rigidbody.drag = SlideDrag;

            //Fuerza en la direccion actual
            rigidbody.AddForce(new Vector2(SlideForce * direction, 0f), ForceMode2D.Impulse);
        }
    }

    /*public void Shift(InputAction.CallbackContext context)
    {
        isShift = context.performed;
    }*/

}
