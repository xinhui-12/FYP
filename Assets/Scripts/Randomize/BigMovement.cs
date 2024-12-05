
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class BigMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDistance = 5f;
    [HideInInspector]
    public float maxSpeed;

    [Header("Move Direction")]
    public bool xMove = false;
    public bool yMove = false;
    public bool zMove = false;

    public bool isNegative = false;

    [Header("Script")]
    public RulerSlash rulerSlash;
    public WordCheck wordCheck;

    private Vector3 startPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position;
        maxSpeed = moveSpeed;
    }

    void Update()
    {
        moveSpeed = maxSpeed * (1 - Mathf.Clamp01((float)rulerSlash.slashTime / wordCheck.word));
        if (moveSpeed == 0) return;

        elapsedTime += Time.deltaTime * moveSpeed;
        float offset = Mathf.PingPong(elapsedTime, moveDistance);
        if (isNegative)
            offset = -offset;

        Vector3 newPosition = startPosition;
        if (xMove)
            newPosition.x += offset;
        if (yMove)
            newPosition.y += offset;
        if (zMove)
            newPosition.z += offset;
        transform.position = newPosition;
    }
}
