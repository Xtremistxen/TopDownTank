using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject topDoor;
    [SerializeField] private GameObject bottomDoor;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [Header("Optional spawn points inside this room")]
    public List<Transform> spawnPoints = new List<Transform>();

    public Vector2Int RoomIndex { get; set; }

    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up && topDoor)
            topDoor.SetActive(true);
        else if (direction == Vector2Int.down && bottomDoor)
            bottomDoor.SetActive(true);
        else if (direction == Vector2Int.left && leftDoor)
            leftDoor.SetActive(true);
        else if (direction == Vector2Int.right && rightDoor)
            rightDoor.SetActive(true);
    }

    public void CloseAllDoors()
    {
        if (topDoor) topDoor.SetActive(false);
        if (bottomDoor) bottomDoor.SetActive(false);
        if (leftDoor) leftDoor.SetActive(false);
        if (rightDoor) rightDoor.SetActive(false);
    }
    
     // Fallback if no explicit spawn points: center of the room
    public Vector3 GetRandomSpawnPosition(Vector2 roomSize)
    {
        if (spawnPoints != null && spawnPoints.Count > 0)
            return spawnPoints[Random.Range(0, spawnPoints.Count)].position;

        return transform.position; // center fallback
    }
}
