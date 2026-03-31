using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    InputSystem_Actions inputSystem;


    [Header("Movement: ")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] float movementSpeed = 5f;
    float lastInputX;
    float lastInputY;

    [Header("Interact: ")]
    [SerializeField] Collider interactArea;
    [SerializeField] LayerMask interactableLayer;
    private IInteractable currentTarget;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    void Start()
    {
        
    }

    void Update()
    {
        moveInput = inputSystem.Player.Move.ReadValue<Vector2>().normalized;
        Vector3 scale = transform.localScale;

        if (moveInput != Vector2.zero)
        {
            lastInputX = moveInput.x;
            lastInputY = moveInput.y;
        }

        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if(inputSystem.Player.Interact.WasPressedThisFrame())
        {
            currentTarget?.OnInteract();
        }

        if(inputSystem.Player.Attack.WasPressedThisFrame())
        {
            animator.SetTrigger("attack");
        }

        SetAnimatorValue();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.y) * movementSpeed;
    }

    void SetAnimatorValue()
    {
        animator.SetFloat("inputX", inputSystem.Player.Move.ReadValue<Vector2>().x);
        animator.SetFloat("inputY", inputSystem.Player.Move.ReadValue<Vector2>().y);
        animator.SetFloat("lastInputX", lastInputX);
        animator.SetFloat("lastInputY", lastInputY);
        animator.SetFloat("speed", rb.linearVelocity.sqrMagnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            Debug.Log("can interact");
            if (other.TryGetComponent(out IInteractable interactable))
            {
                currentTarget = interactable;
                Debug.Log("ahve iinteractable");
            }
        }
    }

    private void OnEnable()
    {
        inputSystem.Player.Enable();
    }
    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }
}
