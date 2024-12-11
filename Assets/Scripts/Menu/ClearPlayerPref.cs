
using UnityEngine;

public class ClearPlayerPref : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared!");
    }
}
