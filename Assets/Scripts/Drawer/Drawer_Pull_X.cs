
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Drawer_Pull_X : MonoBehaviour
{

    public Animator pull_01;
    public bool open;
    public Transform Player;
    public LightSwitchInteractable lightSwitchInteractable;
    private XRSimpleInteractable simpleInteractable;

    void Start()
    {
        open = false;

        simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (!lightSwitchInteractable.lightSwitchAnimation.GetBool("SwitchOpen"))
            return;
        // Check distance to the player
        float dist = Vector3.Distance(Player.position, transform.position);
        if (dist < 10)
        {
            // Open or close depending on the current state
            if (!open)
            {
                StartCoroutine(Opening());
            }
            else
            {
                StartCoroutine(Closing());
            }
        }
    }

    private void OnDestroy()
    {
        simpleInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    IEnumerator Opening()
    {
        pull_01.Play("openpull_01");
        open = true;
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Closing()
    {
        pull_01.Play("closepush_01");
        open = false;
        yield return new WaitForSeconds(0.5f);
    }
}
