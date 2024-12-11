
using UnityEngine;

public class ActivateRay : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject albumOptions;
    public GameObject settingOptions;
    public GameObject dialogWarning;

    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;

    void Update()
    {
        if(pauseMenu.activeSelf || albumOptions.activeSelf || settingOptions.activeSelf || dialogWarning.activeSelf)
        {
            leftRayInteractor.SetActive(true);
            rightRayInteractor.SetActive(true);
        }
        else
        {
            leftRayInteractor.SetActive(false);
            rightRayInteractor.SetActive(false);
        }
    }
}
