
using UnityEngine;

public class ClearPlayerPref : MonoBehaviour
{
    // Static flag to check if the function has already run
    private static bool hasInitialized = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!hasInitialized)
        {
            // Run the function only once when the application starts
            ClearAllPlayerPref();
            hasInitialized = true;
        }
    }

    private void ClearAllPlayerPref()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared!");
    }
}
