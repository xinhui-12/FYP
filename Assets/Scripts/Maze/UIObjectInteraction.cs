
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIObjectInteraction : MonoBehaviour
{
    public Maze maze;
    public GameObject player;
    private Vector3 lastInteractionPosition;
    public GameObject number;

    void Start()
    {
        GameObject endWall = maze.gameObject.transform.Find("EndWall").gameObject;
        if(endWall != null)
        {
            endWall.AddComponent<MazeEndTrigger>().Initialize(this);
        }
    }

    public void OnMazeImageClicked(SelectEnterEventArgs args)
    {
        if (!PauseMenu.pause)
        {
            
            lastInteractionPosition = player.transform.position;
            player.transform.position = maze.startPosition;
        }
            
    }

    public void ResetPlayerPosition()
    {
        player.transform.position = lastInteractionPosition;
        number.SetActive(true);
    }
}