
using UnityEngine;

public class CanvaRelocate : MonoBehaviour
{
    private Vector3 originalPosition;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(rectTransform.localPosition.y < -0.5f)
        {
            rectTransform.localPosition = originalPosition;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
