
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScare : MonoBehaviour
{
    public GameObject picture;
    public Transform player;
    public Maze maze;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            picture.SetActive(true);
            // Cannot back to original place
            Invoke(nameof(PlayerBackToStart), 2);
        }
    }
    private void PlayerBackToStart()
    {
        player.position = new Vector3(maze.startPosition.x, maze.startPosition.y + 1, maze.startPosition.z);
        picture?.SetActive(false);
    }
}
