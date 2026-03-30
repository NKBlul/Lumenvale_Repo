using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    InputSystem_Actions inputSystem;


    [Header("Movement:")]
    [SerializeField] Vector2 moveInput;
    [SerializeField] float movementSpeed = 5f;

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
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.y) * movementSpeed;
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
