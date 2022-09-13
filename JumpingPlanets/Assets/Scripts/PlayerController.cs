using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    public static PlayerController Instance { get; private set; }
    [Header("Movement Variables")]
    [Tooltip("Player Speed")]
    [SerializeField] private float speed;
    [Tooltip("Player jump force")]
    [SerializeField] private float jumpForce;

    [Space]
    [Tooltip("Layer for ground check")]
    [SerializeField] private LayerMask planetLayer;

    private PlayerInputActions InputAction;
    private float input;
    private Rigidbody2D rBody;
    private PlayerState currentState = PlayerState.Floating;

    #endregion Fields


    private void Awake()
    {
        InputAction = new PlayerInputActions();

        InputAction.Player.Movement.performed += ctx =>
        {
            input = ctx.ReadValue<float>();
        };
        InputAction.Player.Movement.canceled += ctx =>
        {
            input = 0;
        };

        InputAction.Player.Jump.performed += ctx => Jump();
    }
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        if (input != 0)
            rBody.AddForce(transform.right * input * speed);
    }

    private void OnEnable()
    {
        InputAction.Enable();
    }

    private void OnDisable()
    {
        InputAction.Disable();
    }

    private void Jump()
    {
        if(currentState == PlayerState.Grounded)
        {
            rBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            currentState = PlayerState.Floating;
        }
    }

    void CheckGround()
    {
        //Check only the planet's collider that is not triggered. In this case we can also use a simple ground check using distance from planet
        RaycastHit2D[] allRaycasts = Physics2D.RaycastAll(transform.position, -transform.up, 1.5f, planetLayer);
        foreach (var item in allRaycasts)
        {
            if(!item.collider.isTrigger)
            {
                currentState = PlayerState.Grounded;
                return;
            }             
        }
        currentState = PlayerState.Floating;
    }


    private enum PlayerState
    {
        Grounded, Floating
    }
}
