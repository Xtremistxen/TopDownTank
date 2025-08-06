using UnityEngine;

public class RoomTile : MonoBehaviour
{
    public GameObject northDoor, southDoor, eastDoor, westDoor;

    public void SetDoors(bool north, bool south, bool east, bool west)
    {
        if (northDoor) northDoor.SetActive(north);
        if (southDoor) southDoor.SetActive(south);
        if (eastDoor)  eastDoor.SetActive(east);
        if (westDoor)  westDoor.SetActive(west);
    }
}

