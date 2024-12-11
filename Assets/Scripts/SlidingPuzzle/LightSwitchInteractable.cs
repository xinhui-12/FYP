
using System.Collections;
using TMPro;
using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour
{
    public Animator lightSwitchAnimation;
    public Light lightToOpen;
    public AnimationCurve lightIntensityCurve;
    public TMP_Text notepad;
    public PhotoGallery photoGallery;
    private bool firstTimeOpen = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            bool isSwitchOpen = !lightSwitchAnimation.GetBool("SwitchOpen");
            lightSwitchAnimation.SetBool("SwitchOpen", isSwitchOpen);
            StartCoroutine(ControlLightSwitch(isSwitchOpen));
            if (firstTimeOpen && isSwitchOpen)
                photoGallery.UnlockPhoto(photoGallery.customOrder[3]);
        }
    }

    private IEnumerator ControlLightSwitch(bool isTurningOn)
    {
        float animationDuration = lightSwitchAnimation.GetCurrentAnimatorStateInfo(0).length;
        float elapsedTime = 0f;
        lightToOpen.intensity = isTurningOn ? 0 : 1;

        while (elapsedTime < animationDuration)
        {
            float normalizedTime = elapsedTime / animationDuration;
            lightToOpen.intensity = isTurningOn
                ? lightIntensityCurve.Evaluate(normalizedTime) // Increase intensity if turning on
                : lightIntensityCurve.Evaluate(1f - normalizedTime); // Decrease intensity if turning off

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final light state is correctly set after the animation completes
        lightToOpen.intensity = isTurningOn ? lightIntensityCurve.Evaluate(1f) : 0f;
        notepad.gameObject.SetActive(isTurningOn);
    }
}
