using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Prefabs & Sizes")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private int roomWidth = 20;
    [SerializeField] private int roomHeight = 12;

    [Header("Generation Limits")]
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 7;

    [Header("Grid")]
    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;

    public enum GenerationMode { Random, PresetSeed, LevelOfTheDay }

    [Header("Generation Mode")]
    [SerializeField] private GenerationMode generationMode = GenerationMode.Random;
    [SerializeField] private string presetSeed = "UAT-205";  // editable in Inspector

    private void ApplySeed()
    {
        int seed;
        switch (generationMode)
        {
            case GenerationMode.PresetSeed:
                seed = StableStringHash(presetSeed);
                break;
            case GenerationMode.LevelOfTheDay:
                var today = System.DateTime.UtcNow.Date;
                seed = today.GetHashCode();
                break;
            default:
                seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                break;
        }

        UnityEngine.Random.InitState(seed);
    }

    private int StableStringHash(string s)
    {
        unchecked
        {
            int hash = 23;
            for (int i = 0; i < (s?.Length ?? 0); i++)
                hash = hash * 31 + s[i];
            return hash;
        }
    }

    

    private readonly List<GameObject> roomObjects = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    private int[,] roomGrid; // 0 = empty, 1 = room exists
    private int roomCount;
    private bool generationComplete;

    void Start()
    {
        ApplySeed();
        RegenerateRooms(); // initializes everything and starts from center
    }

    void Update()
    {
        if (generationComplete) return;

        if (roomQueue.Count > 0 && roomCount < maxRooms)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int x = roomIndex.x;
            int y = roomIndex.y;

            // Try to branch in 4 directions from the current room
            TryGenerateRoom(new Vector2Int(x - 1, y)); // left
            TryGenerateRoom(new Vector2Int(x + 1, y)); // right
            TryGenerateRoom(new Vector2Int(x, y + 1)); // up
            TryGenerateRoom(new Vector2Int(x, y - 1)); // down
        }
        else if (roomQueue.Count == 0)
        {
            // Finished growth
            if (roomCount < minRooms)
            {
                Debug.Log("Room count below minimum. Regenerating…");
                RegenerateRooms();
                return;
            }

            Debug.Log($"Generation complete, {roomCount} rooms created");
            generationComplete = true;
        }
    }

    
    void RegenerateRooms()
    {
        // Clean old
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();

        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();
        roomCount = 0;
        generationComplete = false;

        // Start from center of the grid
        Vector2Int start = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(start);
    }

    void StartRoomGenerationFromRoom(Vector2Int idx)
    {
        if (!InBounds(idx)) return;

        roomQueue.Enqueue(idx);
        roomGrid[idx.x, idx.y] = 1;
        roomCount++;

        var roomGO = Instantiate(roomPrefab, GetPositionFromGridIndex(idx), Quaternion.identity);
        roomGO.name = $"Room-{roomCount}";
        var room = roomGO.GetComponent<Room>();
        room.RoomIndex = idx;
        room.CloseAllDoors(); // start closed by default
        roomObjects.Add(roomGO);

        // No doors to open yet; neighbors get opened as they’re created
    }

    bool TryGenerateRoom(Vector2Int idx)
    {
        // Stop if we’re done
        if (roomCount >= maxRooms) return false;

        // Bounds
        if (!InBounds(idx)) return false;

        int x = idx.x;
        int y = idx.y;

        // Already has a room? skip
        if (roomGrid[x, y] != 0) return false;

        // Random gate (tweak density): ~45% chance to grow
        if (Random.value >= 0.45f) return false;

        // Force a more “stringy” layout: avoid junctions > 2
        if (CountAdjacentRooms(idx) > 1) return false;

        // Mark & enqueue
        roomQueue.Enqueue(idx);
        roomGrid[x, y] = 1;
        roomCount++;

        // Create room
        var newRoomGO = Instantiate(roomPrefab, GetPositionFromGridIndex(idx), Quaternion.identity);
        newRoomGO.name = $"Room-{roomCount}";
        var newRoom = newRoomGO.GetComponent<Room>();
        newRoom.RoomIndex = idx;
        newRoom.CloseAllDoors();
        roomObjects.Add(newRoomGO);

        // Open doors between this room and any existing neighbors
        OpenMutualDoors(idx, Vector2Int.left);
        OpenMutualDoors(idx, Vector2Int.right);
        OpenMutualDoors(idx, Vector2Int.up);
        OpenMutualDoors(idx, Vector2Int.down);

        return true;
    }


    // Doors

    void OpenMutualDoors(Vector2Int center, Vector2Int dir)
    {
        Vector2Int neighborIdx = center + dir;
        if (!InBounds(neighborIdx)) return;

        // Only open if neighbor is already created
        if (roomGrid[neighborIdx.x, neighborIdx.y] == 0) return;

        Room thisRoom = GetRoomScriptAt(center);
        Room neighborRoom = GetRoomScriptAt(neighborIdx);

        if (thisRoom != null)
            thisRoom.OpenDoor(dir);

        if (neighborRoom != null)
            neighborRoom.OpenDoor(-dir);
        // If neighborRoom is null, it will open back to this room when it spawns
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        // Linear search is fine for small counts; replace with 2D array if you prefer.
        var found = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        return found ? found.GetComponent<Room>() : null;
    }


    // Helpers

    int CountAdjacentRooms(Vector2Int idx)
    {
        int count = 0;
        if (InBounds(idx + Vector2Int.left) && roomGrid[idx.x - 1, idx.y] != 0) count++;
        if (InBounds(idx + Vector2Int.right) && roomGrid[idx.x + 1, idx.y] != 0) count++;
        if (InBounds(idx + Vector2Int.down) && roomGrid[idx.x, idx.y - 1] != 0) count++;
        if (InBounds(idx + Vector2Int.up) && roomGrid[idx.x, idx.y + 1] != 0) count++;
        return count;
    }

    bool InBounds(Vector2Int idx)
    {
        return idx.x >= 0 && idx.x < gridSizeX && idx.y >= 0 && idx.y < gridSizeY;
    }

    Vector3 GetPositionFromGridIndex(Vector2Int idx)
    {
        // Centered grid: (0,0) is middle of the overall layout
        return new Vector3(
            roomWidth * (idx.x - gridSizeX / 2),
            roomHeight * (idx.y - gridSizeY / 2),
            0f
        );
    }

    public bool IsGenerationComplete => generationComplete;

public List<Room> GetAllRooms()
{
    var list = new List<Room>();
    foreach (var go in roomObjects)
    {
        if (go)
        {
            var r = go.GetComponent<Room>();
            if (r) list.Add(r);
        }
    }
    return list;
}

public Room GetRandomRoom()
{
    var rooms = GetAllRooms();
    if (rooms.Count == 0) return null;
    return rooms[Random.Range(0, rooms.Count)];
}


    void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
    
    
}
