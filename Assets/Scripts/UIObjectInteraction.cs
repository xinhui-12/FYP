
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIObjectInteraction : MonoBehaviour
{
    public Maze maze;
    public GameObject player;
    private Vector3 lastInteractionPosition;

    void Start()
    {
        GameObject endWall = maze.gameObject.transform.Find("EndWall").gameObject;
        if(endWall != null)
        {
            endWall.AddComponent<MazeEndTrigger>().Initialize(this);
        }
    }

    public void onMazeImageClicked(SelectEnterEventArgs args)
    {
        Debug.Log("MazeImage clicked!");
        lastInteractionPosition = player.transform.position;
        player.transform.position = maze.endPosition;
    }

    public void resetPlayerPosition()
    {
        Debug.Log("Returning to last interacted position.");
        player.transform.position = lastInteractionPosition;
    }
}
