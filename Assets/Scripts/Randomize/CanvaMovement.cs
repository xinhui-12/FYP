
using UnityEngine;

public class CanvaMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    [HideInInspector]
    public float maxSpeed;

    public float xMin = -1f;
    public float xMax = 1f;
    public float yMin = -1f;
    public float yMax = 1f;

    [Header("Script")]
    public RulerSlash rulerSlash;
    public WordCheck wordCheck;

    [HideInInspector]
    public RectTransform rectTransform;
    private Vector2 targetPosition;

    void Start()
    {
        maxSpeed = moveSpeed;
        rectTransform = GetComponent<RectTransform>();
        SetRandomTargetPosition();
    }

    void Update()
    {
        moveSpeed = maxSpeed * (1 - Mathf.Clamp01((float)rulerSlash.slashTime / wordCheck.word.Length));
        if (moveSpeed == 0) return;

        // Move the UI element towards the target position
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Check if the element has reached the target position
        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
        {
            // Set a new random target position
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        // Generate a random position within the specified range
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        targetPosition = new Vector2(randomX, randomY);
    }
}