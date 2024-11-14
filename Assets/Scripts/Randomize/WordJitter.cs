
using UnityEngine;

public class WordJitter : MonoBehaviour
{
    public float rotateSpeed = 1f;
    public float rotateAmount = 3f;

    [HideInInspector]
    public bool isTrigger = false;
    private float originalZRotation;

    void Start()
    {
        originalZRotation = transform.eulerAngles.z;
    }

    void Update()
    {
        if (isTrigger) return;
        float swayRotation = Mathf.Sin(Time.time * rotateSpeed) * rotateAmount;
        transform.localRotation = Quaternion.Euler(0, 0, originalZRotation + swayRotation);
    }
}
