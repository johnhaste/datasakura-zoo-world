using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public float speed = 5f;
    public MovementDirection movementDirection = MovementDirection.FreeMovement;
    private Vector3 currentDirection;
    public float directionChangeInterval = 5f; // Interval to randomly change direction unless moving to center
    private float directionChangeTimer;
    private bool movingToCenter = false;
    private Vector3 centerPosition = Vector3.zero; // Assuming center is (0,0,0)

    void Start()
    {
        ChooseNewDirection();
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
        Move();

        if (!movingToCenter)
        {
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0)
            {
                ChooseNewDirection();
                directionChangeTimer = directionChangeInterval;
            }
        }
    }

    private void Move()
    {
        transform.position += currentDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.gameObject.tag);

        if (other.gameObject.CompareTag("StageBounds") && !movingToCenter)
        {
            // Change direction to move towards the center of the stage
            currentDirection = (centerPosition - transform.position).normalized;
            movingToCenter = true;
        }
        else if (other.gameObject.CompareTag("PlayableArea") && movingToCenter)
        {
            // Reset movement to normal random direction upon entering playable area
            movingToCenter = false;
        }
    }

    private void ChooseNewDirection()
    {
        currentDirection = CalculateDirection().normalized;
        currentDirection.y = 0; // Ensure there's no vertical movement
    }

    private Vector3 CalculateDirection()
    {
        if (movingToCenter)
        {
            return centerPosition - transform.position; // Direction towards the center
        }
        else
        {
            // Random direction based on the enum selection
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
                    z = Random.value < 0.5 ? x : -x;
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
}