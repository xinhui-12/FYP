
using UnityEngine;

public class CheatArrange : MonoBehaviour
{
    public CertificatePosition certificatePosition;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            for (int i = 0; i < certificatePosition.objects.Length; i++)
            {
                certificatePosition.objects[i].position = certificatePosition.snapPoints[i].position;
                certificatePosition.snapPointOccupancy[certificatePosition.snapPoints[i]] = certificatePosition.objects[i];
            }
            certificatePosition.CheckCorrectPosition();
        }
    }
}
