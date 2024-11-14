
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    [HideInInspector]
    public float maxSpeed;

    public Vector3 minMovement;
    public Vector3 maxMovement;

    [Header("Script")]
    public RulerSlash rulerSlash;
    public WordCheck wordCheck;

    private Vector3 targetPosition;

    void Start()
    {
        maxSpeed = moveSpeed;
        SetRandomTargetPosition();
    }

    void Update()
    {
        moveSpeed = maxSpeed * (1 - Mathf.Clamp01((float)rulerSlash.slashTime / wordCheck.word.Length));
        if (moveSpeed == 0) return;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        Vector3 randomPosition;
        randomPosition.x = Random.Range(minMovement.x, maxMovement.x);
        randomPosition.y = Random.Range(minMovement.y, maxMovement.y);
        randomPosition.z = Random.Range(minMovement.z, maxMovement.z);
        targetPosition = randomPosition;
    }
}
