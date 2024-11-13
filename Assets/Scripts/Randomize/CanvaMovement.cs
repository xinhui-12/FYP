using UnityEngine;

public class CanvaMovement : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed of the movement

    public float xMin = -1f;
    public float xMax = 1f;
    public float yMin = -1f;
    public float yMax = 1f;

    private RectTransform rectTransform;
    private Vector2 targetPosition;
    private Vector2 originalPosition;

    void Start()
    {
        // Get the RectTransform component
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        // Set an initial random target position
        SetRandomTargetPosition();
    }

    void Update()
    {
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