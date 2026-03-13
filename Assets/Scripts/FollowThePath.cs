using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class FollowThePath : MonoBehaviour {

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;

    private int waypointIndex = 0;
    private bool isChasing = false;
    private Transform playerTransform;

    // --- Naye Animation Variables ---
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start () {
        transform.position = waypoints[waypointIndex].transform.position;
        
        // Components fetch kar rahe hain
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update () {
        Move();
    }

    private void Move()
    {
        Vector2 targetPosition; // Hum target position ko ek variable mein save karenge

        if (isChasing && playerTransform != null)
        {
            // Chase mode
            targetPosition = playerTransform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
        }
        else
        {
            // Patrol mode
            if (waypointIndex <= waypoints.Length - 1)
            {
                targetPosition = waypoints[waypointIndex].transform.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
                {
                    waypointIndex = (waypointIndex + 1) % waypoints.Length;
                }
            }
            else
            {
                return; // Agar koi waypoint nahi hai toh return kar jao
            }
        }

        // --- ANIMATION LOGIC ---
        // 1. Direction Calculate karo (Target - Current Position)
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // 2. Sprite Flip Logic (Method 1)
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true; // Right
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false; // Left
        }

        // 3. Animator ko values bhejo
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        
        // Kyunki Enemy hamesha chal raha hai (kabhi Idle nahi hota), hum Speed ko hamesha 1 bhej sakte hain
        // Taaki "Run" wala Blend Tree humesha play hota rahe.
        animator.SetFloat("Speed", 1f); 
    }

    public void StartChase(Transform player)
    {
        isChasing = true;
        playerTransform = player;
    }

    public void StopChase()
    {
        isChasing = false;
        playerTransform = null;

        // Find the closest waypoint to current position
        FindClosestWaypoint();
    }

    // Find the closest waypoint and set it as the target
    private void FindClosestWaypoint()
    {
        float closestDistance = float.MaxValue;
        int closestWaypointIndex = 0;

        // Check distance to all waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, waypoints[i].position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypointIndex = i;
            }
        }

        // Set the closest waypoint as the current target
        waypointIndex = closestWaypointIndex;
        Debug.Log("Enemy found closest waypoint at index: " + closestWaypointIndex + " (distance: " + closestDistance + ")");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    public void SetWaypoints(Transform[] newWaypoints, int startingWaypointIndex)
    {
        waypoints = newWaypoints;
        waypointIndex = startingWaypointIndex;
        transform.position = waypoints[waypointIndex].position;
    }
}