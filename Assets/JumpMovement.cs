using UnityEngine;
using System.Collections;

public class JumpMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;

    [Header("Control Settings")]
    private bool alive = true;
    public float jumpInterval = 2f; // Seconds between each jump
    public float jumpForceMin = 5f; // Minimum jump force
    public float jumpForceMax = 10f; // Maximum jump force
    public MovementDirection movementDirection = MovementDirection.HorizontalAndVertical;
    public float rotationSpeed = 2f; // Speed of rotation

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
            yield return new WaitForSeconds(jumpInterval); 
            Jump();
        }
    }

    //Jump according to parameters
    private void Jump()
    {
        //Deciddes which direction to jump
        Vector3 jumpDirection = CalculateJumpDirection();

        //Determines the force of the jump
        float jumpForce = Random.Range(jumpForceMin, jumpForceMax);

        //Rotates the object towards the jump direction
        StartCoroutine(SmoothRotateTowards(jumpDirection));
        
        //Takes the player off the ground
        jumpDirection += Vector3.up;

        //Applies the jump force
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
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
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
