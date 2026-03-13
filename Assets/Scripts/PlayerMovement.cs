using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))] // Animator ensure kar rahe hain
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator; // Animator variable
    private Vector2 movement;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Animator ko fetch kiya

        moveAction = new InputAction(type: InputActionType.Value, expectedControlType: "Vector2");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
        moveAction.Enable();
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        movement = movement.normalized;

        // Sirf tabhi direction update karo jab player actual mein move kar raha ho
        // Isse "last direction" Animator mein save reh jayegi!
        if (movement.sqrMagnitude > 0)
        {
            // Sprite Flip Logic
            if (movement.x > 0)
            {
                spriteRenderer.flipX = true; // Right face karega
            }
            else if (movement.x < 0)
            {
                spriteRenderer.flipX = false; // Left face karega
            }

            // Animator Direction Parameters
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        // Speed hamesha update hogi taaki 0 hone par Idle animation play ho sake
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        moveAction?.Dispose();
    }
}