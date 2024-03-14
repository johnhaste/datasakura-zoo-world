using System.Collections;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;

    public float speed = 5f;
    public MovementDirection movementDirection = MovementDirection.FreeMovement;
    private Vector3 currentDirection;
    public float directionChangeInterval = 5f; // Interval to randomly change direction
    public float rotationSpeed = 2f; // Adjust this for a smoother rotation

    public Vector3 centerPosition = Vector3.zero; // Assuming center is (0,0,0)
    public bool isMovingToCenter = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChooseNewDirection();
        StartCoroutine(DirectionChangeRoutine());
    }

    void FixedUpdate() // Move physics updates to FixedUpdate
    {
        Move(); // We'll adjust the Move method to use Rigidbody velocity
        RotateTowardsDirection();
    }

    private void Move()
    {
        // Update the Rigidbody's velocity instead of transform position
        rb.velocity = currentDirection * speed;
    }

    public void RotateTowardsDirection()
    {
        if (currentDirection != Vector3.zero) // Check to prevent "Look rotation viewing vector is zero" error
        {
            Vector3 horizontalDirection = new Vector3(currentDirection.x, 0, currentDirection.z).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
            // Smoothly rotate towards the target direction
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator DirectionChangeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);

            if (!isMovingToCenter)
            {
                ChooseNewDirection();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StageBounds"))
        {
            isMovingToCenter = true;
            currentDirection = (centerPosition - transform.position).normalized;
        }
        else if (other.gameObject.CompareTag("PlayableArea"))
        {
            isMovingToCenter = false;
            StartCoroutine(WaitAndChooseNewDirection(1));
        }
    }

    IEnumerator WaitAndChooseNewDirection(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChooseNewDirection(); // Resume normal movement upon entering playable area
    }

    private void ChooseNewDirection()
    {
        currentDirection = CalculateDirection().normalized;
        currentDirection.y = 0; // Ensure there's no vertical movement
    }

    private Vector3 CalculateDirection()
    {
        float x = 0, z = 0;
        switch (movementDirection)
        {
            case MovementDirection.OnlyHorizontal:
                x = Random.Range(-1f, 1f);
                z = 0;
                break;
            case MovementDirection.OnlyVertical:
                x = 0;
                z = Random.Range(-1f, 1f);
                break;
            case MovementDirection.OnlyDiagonals:
                x = Random.Range(-1f, 1f);
                z = Random.value < 0.5 ? x : -x; // Ensuring diagonal movement
                break;
            case MovementDirection.HorizontalAndVertical:
                if (Random.value < 0.5f)
                {
                    x = Random.Range(-1f, 1f);
                    z = 0;
                }
                else
                {
                    x = 0;
                    z = Random.Range(-1f, 1f);
                }
                break;
            case MovementDirection.HorizontalVerticalAndDiagonals:
                // This allows for any direction, but it's not purely random across all vectors like FreeMovement
                x = Random.Range(-1f, 1f);
                z = Random.Range(-1f, 1f);
                break;
            case MovementDirection.FreeMovement:
                x = Random.Range(-1f, 1f);
                z = Random.Range(-1f, 1f);
                break;
        }
        return new Vector3(x, 0, z);
    }

}
