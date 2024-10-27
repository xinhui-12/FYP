
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UIObjectInteraction : MonoBehaviour
{
    public Maze maze;
    public GameObject player;

    void Start()
    {
        
    }

    public void onMazeImageClicked(SelectEnterEventArgs args)
    {
        Debug.Log("MazeImage clicked!");
        player.transform.position = maze.startPosition;

    }
}
