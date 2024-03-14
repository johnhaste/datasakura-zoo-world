using UnityEngine;
using System.Collections;

public class JumpMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;

    [Header("Control Settings")]
    private bool alive = true;
    public float jumpIntervalMin = 2f; // Seconds between each jump
    public float jumpIntervalMax = 4f; // Seconds between each jump
    public float jumpForce = 5f; // Minimum jump force
    public MovementDirection movementDirection = MovementDirection.HorizontalAndVertical;
    public float rotationSpeed = 2f; // Speed of rotation

    [Header("Off Bounds Control")]
    private Vector3 centerStage = Vector3.zero; // Center of the stage
    private bool aimTowardsCenterNextJump = false; // Whether to aim towards the center on the next jump

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(JumpRoutine());
    }

    //Jumping after X Seconds
    IEnumerator JumpRoutine()
    {
        while (alive)
        {
            yield return new WaitForSeconds(Random.Range(jumpIntervalMin, jumpIntervalMax)); 
            Jump();
        }
    }

    //Jump according to parameters
    private void Jump()
    {
        Vector3 jumpDirection = aimTowardsCenterNextJump ? (centerStage - transform.position).normalized : CalculateJumpDirection();

        // Reset aim towards center flag after it's used
        aimTowardsCenterNextJump = false;

        StartCoroutine(SmoothRotateTowards(jumpDirection));
        jumpDirection += Vector3.up;
        rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
    }


    private Vector3 CalculateJumpDirection()
    {
        float x = 0, z = 0;

        switch (movementDirection)
        {
            case MovementDirection.OnlyHorizontal:
                x = Random.Range(-1f, 1f);
                break;
            case MovementDirection.OnlyVertical:
                z = Random.Range(-1f, 1f);
                break;
            case MovementDirection.OnlyDiagonals:
                x = Random.Range(-1f, 1f);
                z = Random.value < 0.5 ? x : -x; // Ensures diagonal movement
                break;
            case MovementDirection.HorizontalAndVertical:
                if (Random.value < 0.5f) x = Random.Range(-1f, 1f); else z = Random.Range(-1f, 1f);
                break;
            case MovementDirection.HorizontalVerticalAndDiagonals:
                x = Random.Range(-1f, 1f);
                z = Random.Range(-1f, 1f);
                break;
            case MovementDirection.FreeMovement:
                x = Random.Range(-1f, 1f);
                z = Random.Range(-1f, 1f);
                break;
        }

        return new Vector3(x, 0, z).normalized;
    }

    IEnumerator SmoothRotateTowards(Vector3 direction)
    {
        // Calculate the horizontal direction vector by ignoring the y component
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;

        // Calculate target rotation based on the horizontal direction only
        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter:" + collider.gameObject.tag);

        if (collider.gameObject.CompareTag("StageBounds"))
        {
            aimTowardsCenterNextJump = true;
        }
    }

    public void ToggleAlive(bool toggle)
    {
        alive = toggle;
    }
}

public enum MovementDirection
{
    OnlyHorizontal,
    OnlyVertical,
    OnlyDiagonals,
    HorizontalAndVertical,
    HorizontalVerticalAndDiagonals,
    FreeMovement
}
