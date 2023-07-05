using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float HorizontalVelocty;
    [SerializeField] private float VerticalVelocty;
    [SerializeField] private float FallForce;
    [SerializeField] private float SlideForce;
    [SerializeField] private float SlideDrag;
    [SerializeField] private float WallJumpForce;
    [SerializeField] private float WallJumpMaxFallVelocity;
    [SerializeField] private float IceSlideForce;
    [SerializeField] private Animator Animator;
    [SerializeField] private Collider2D[] animationColiders; //Orden: [0]Idle/Jump [1]Run/AirDash [2]Slide/Shift

    private new Rigidbody2D rigidbody;
    private LayerMask layerGround;
    private LayerMask layerWallJump;
    private Collider2D rayCollider;

    public bool canJump { get; private set; }    //Si puede saltar.
    private bool wasJumping; //Si estaba saltado. [Coliders]
    private bool wantJump;
    public bool canSlide { get; private set; }
    private bool canMoveForward;    //Detecta si hay pared
    private bool firstWallJump;     //Evita el salto repetido en la pared
    private bool firstSlide;        //Evita el doble slide de pared
    private bool canMove;
    private bool canFastFall;
    private bool isIceSliding;

    private float raycastGroundDistance;
    private float raycastHorizontalDistance;
    private int extraJumps;
    private int extraWallJumps;
    private float jumpDealy;
    private float speed;        //Velocidad en teoria
    private float auxSpeed;     //Evita bugs
    private float direction;    //Direccion actual
    private float auxDirection; //Evita bugs
    private float timerWallGrap;
    private float timerWallMove;

    private int actIndex;       //Indice del colider actual

    private PlayerProp playerProp;

    //TODO:
    // - Implementar el wall jump.
    // - Ajustar el personaje al mapa [Primero tener mapa]
    // - Arreglar hitbox y centro de run
    // - Animacion fall

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        layerGround = LayerMask.GetMask("Ground");
        layerWallJump = LayerMask.GetMask("Wall Jump");  
        raycastGroundDistance = 1.7f;       //Mejor distancia al suelo
        raycastHorizontalDistance = 0.8f;  //Mejor distancia a la pared

        canSlide = true;
        canMoveForward = true;
        actIndex = 0;

        ReloadPlayer();
    }
    public void ReloadPlayer()
    {
        playerProp = GameManager.Instance.GetPropertys;
        extraJumps = playerProp.dobleJump ? 1 : 0;
        extraWallJumps = playerProp.dobleWallJump ? 1 : 0;
    }
    private void DirectionAndFix(int format)
    {
        //Redondear la velocidad [BUG ANIMACIONES]
        string roundVelociadX = rigidbody.velocity.x.ToString("f" + format);
        float fRoundVelociadX = float.Parse(roundVelociadX);
        string roundVelociadY = rigidbody.velocity.y.ToString("f" + format);
        float fRoundVelociadY = float.Parse(roundVelociadY);
        rigidbody.velocity = new Vector2(fRoundVelociadX, fRoundVelociadY);

        direction = Mathf.Round(Mathf.Clamp(transform.eulerAngles.y - 90, -1f, 1f) * -1f); //Direccion del personaje [IZQ](-1f) o [DER](1f)
    }

    private void Raycasts(float distanceHorizontal)
    {
        //Detectar si se pude saltar
        Vector3 raycastHorizontalCenter = transform.position + new Vector3(-0.233f, 0, 0) * direction;
        rayCollider = Physics2D.Raycast(raycastHorizontalCenter, Vector2.down, raycastGroundDistance, layerGround).collider;
        canJump = rayCollider != null && rayCollider.enabled;
        if (!canJump)
        {
            rayCollider = Physics2D.Raycast(raycastHorizontalCenter + new Vector3(-distanceHorizontal, 0, 0), Vector2.down, raycastGroundDistance, layerGround).collider;
            canJump = rayCollider != null && rayCollider.enabled;
        }
        if (!canJump)
        {
            rayCollider = Physics2D.Raycast(raycastHorizontalCenter + new Vector3(distanceHorizontal, 0, 0), Vector2.down, raycastGroundDistance, layerGround).collider;
            canJump = rayCollider != null && rayCollider.enabled;
        }
        if (canJump && extraJumps == 0)
            extraJumps = playerProp.dobleJump ? 1 : 0;
        if (canJump && extraWallJumps == 0)
            extraWallJumps = playerProp.dobleWallJump ? 1 : 0;

        //Detectar si hay una pared
        Vector3 raycastVerticalCenter = transform.position + new Vector3(0, 1.6f, 0);
        canMoveForward = Physics2D.Raycast(raycastVerticalCenter, Vector2.right * direction, raycastHorizontalDistance, layerWallJump).collider == null;

        //Debug Info
        Debug.DrawLine(raycastVerticalCenter, raycastVerticalCenter + Vector3.right * raycastHorizontalDistance * direction);
        Debug.DrawLine(raycastHorizontalCenter, raycastHorizontalCenter + Vector3.down * raycastGroundDistance);
        Debug.DrawLine(raycastHorizontalCenter + new Vector3(-distanceHorizontal, 0, 0), raycastHorizontalCenter + new Vector3(-distanceHorizontal, 0, 0) + new Vector3(0, -raycastGroundDistance, 0));
        Debug.DrawLine(raycastHorizontalCenter + new Vector3(distanceHorizontal, 0, 0), raycastHorizontalCenter + new Vector3(distanceHorizontal, 0, 0) + new Vector3(0, -raycastGroundDistance, 0));
    }

    private void AnimationsSet()
    {
        //Actualizar animaciones
        Animator.SetFloat("Jumping", !canJump ? 1f : 0f);
        Animator.SetBool("Slide", !canSlide);
        Animator.SetBool("Wall", !canJump && !canMoveForward && timerWallGrap == 0f);
        Animator.SetFloat("Jumping Velocity", Mathf.Clamp(rigidbody.velocity.y, -1f, 1f));
        if (isIceSliding)
            Animator.SetFloat("Horizontal Speed", Mathf.Clamp(speed, -1f, 1f));
        else if ((canMoveForward || (!canMoveForward && auxDirection != direction)) && !isIceSliding)
            Animator.SetFloat("Horizontal Speed", Mathf.Clamp(rigidbody.velocity.x, -1f, 1f));
        else
            Animator.SetFloat("Horizontal Speed", 0f);
    }

    void Update()
    {
        DirectionAndFix(2);
        Raycasts(0.55f);
        AnimationsSet();

        //Ice Sliding
        isIceSliding = rayCollider != null && rayCollider.tag == "Ice";
        if (!isIceSliding && !canSlide) rigidbody.drag = SlideDrag;
       
        //Timer
        if (timerWallGrap > 0f)
        {
            timerWallGrap -= Time.deltaTime;
            if (timerWallGrap < 0f) timerWallGrap = 0f;
        }
        if (timerWallMove > 0f)
        {
            timerWallMove -= Time.deltaTime;
            if (timerWallMove < 0f) timerWallMove = 0f;
        }
        if (jumpDealy > 0f)
        {
            jumpDealy -= Time.deltaTime;
            if (jumpDealy < 0f) jumpDealy = 0f;
        }
    }

    private void FixedUpdate()
    {
        //Move and Ice Sliding
        if (canSlide && timerWallMove == 0f && canMove)
        {
            if(isIceSliding)
            {
                rigidbody.drag = 2f;
                if (speed < rigidbody.velocity.x && speed != 0) 
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x - IceSlideForce * (playerProp.plusVelocity ? 1.5f : 1f), rigidbody.velocity.y);
                else if(speed > rigidbody.velocity.x && speed != 0) 
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x + IceSlideForce * (playerProp.plusVelocity ? 1.5f : 1f), rigidbody.velocity.y);
            }
            else
            {
                rigidbody.drag = 0f;
                rigidbody.velocity = new Vector2(speed * (playerProp.plusVelocity ? 1.5f : 1f), rigidbody.velocity.y);
            }
        }
            
        //Rotacion objeto
        if (rigidbody.velocity.x < 0 || (isIceSliding && speed < 0 && canSlide))
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (rigidbody.velocity.x > 0 || (isIceSliding && speed > 0 && canSlide))
            transform.eulerAngles = new Vector3(0, 0, 0);

        //Slide restrictivo
        if (!canSlide && Mathf.Abs(rigidbody.velocity.x) < HorizontalVelocty)
        {
            canSlide = true;
            rigidbody.drag = 0f;
        }
        if (!firstSlide) 
            firstSlide = canJump;

        //Jump y WallJump
        if ((canJump || (extraJumps > 0 && jumpDealy == 0f)) && wantJump && canSlide && canMoveForward) //Salto normal
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * VerticalVelocty * (playerProp.plusJump ? 1.2f : 1f), ForceMode2D.Impulse);
            timerWallGrap = 0.2f;
            if(!canJump) --extraJumps;
            jumpDealy = 0.2f;
            canJump = false;
        }
        if ((!canMoveForward || (extraWallJumps > 0 && !canMoveForward)) && jumpDealy == 0f && wantJump && (firstWallJump || extraWallJumps > 0 ) && timerWallGrap == 0f) //WallJump
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(new Vector2(-0.5f * direction, 1f) * WallJumpForce, ForceMode2D.Impulse);
            if (!firstWallJump) --extraWallJumps;
            firstWallJump = false;
            auxSpeed = speed;
            canMove = false;
            timerWallMove = 0.1f;
            jumpDealy = 0.2f;
        }
        if (timerWallGrap == 0f && !canMoveForward)
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Clamp(rigidbody.velocity.y, -WallJumpMaxFallVelocity, float.MaxValue));
        if (!firstWallJump)
            firstWallJump = canJump;
        if (!canMove)
            canMove = auxSpeed != speed || canJump;

        //Actualizar colider
        if (Mathf.Clamp(speed, -1f, 1f) != 0f || !canJump && Mathf.Clamp(rigidbody.velocity.x, -1f, 1f) != 0f)
        {
            if (!wasJumping && !canSlide)
                ColiderActivator(2);
            else if ((canJump && canSlide || !canSlide && !canJump) && canMoveForward)
                ColiderActivator(1);
            else
                ColiderActivator(0);
        }
        else ColiderActivator(0);
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
            if(!isIceSliding) rigidbody.drag = SlideDrag;

            //Fuerza en la direccion actual
            rigidbody.AddForce(new Vector2(SlideForce * speed/HorizontalVelocty, 0f), ForceMode2D.Impulse);
        }
    }

    public void FastFall(InputAction.CallbackContext context)
    {
        if(canFastFall && context.performed)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(Vector2.down * FallForce, ForceMode2D.Impulse);
            canFastFall = false;
        }
        if(context.ReadValue<float>() == 0f && !canFastFall) canFastFall = true;
    }

    /*public void Shift(InputAction.CallbackContext context)
    {
        isShift = context.performed;
    }*/

}
