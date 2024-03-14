using System.Collections;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public float speed = 5f;
    public MovementDirection movementDirection = MovementDirection.FreeMovement;
    private Vector3 currentDirection;
    public float directionChangeInterval = 5f; // Interval to randomly change direction
    public float rotationSpeed = 2f; // Adjust this for a smoother rotation
    public Vector3 centerPosition = Vector3.zero; // Assuming center is (0,0,0)

    void Start()
    {
        ChooseNewDirection();
        StartCoroutine(DirectionChangeRoutine());
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += currentDirection * speed * Time.deltaTime;

        if (currentDirection != Vector3.zero)
        {
            StartCoroutine(SmoothRotateTowards(currentDirection));
        }
    }

    IEnumerator SmoothRotateTowards(Vector3 direction)
    {
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DirectionChangeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChangeInterval);
            ChooseNewDirection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StageBounds"))
        {
            currentDirection = (centerPosition - transform.position).normalized;
            ChooseNewDirection();
        }
        else if (other.gameObject.CompareTag("PlayableArea"))
        {
            ChooseNewDirection(); // Resume normal movement upon entering playable area
        }
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
                break;
            case MovementDirection.OnlyVertical:
                z = Random.Range(-1f, 1f);
                break;
            case MovementDirection.OnlyDiagonals:
                x = Random.Range(-1f, 1f);
                z = x * (Random.value < 0.5 ? 1 : -1);
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
        return new Vector3(x, 0, z);
    }
}