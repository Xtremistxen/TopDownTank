using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton (optional if you already have one)
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private RoomManager roomManager;     // drag your RoomManager here
    [SerializeField] private Camera mainCamera;            // optional: set follow target after spawn

    [Header("Prefabs")]
    [SerializeField] private GameObject Tank;      // your Player Tank prefab
    [SerializeField] private GameObject AITank;          // your AI Tank prefab (with AITankController)

    [Header("Spawn Settings")]
    [SerializeField] private int aiCount = 3;
    [SerializeField] private Vector2 roomSizeForFallback = new Vector2(20f, 12f); // match your RoomManager sizes
    [SerializeField] private bool separateRooms = true;    // try to put each actor in a different room

    private GameObject playerInstance;
    private readonly List<GameObject> aiInstances = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // uncomment if you change scenes
    }

    void Start()
    {
        if (roomManager == null)
            roomManager = FindObjectOfType<RoomManager>();

        StartCoroutine(SpawnAfterGeneration());
    }

    private IEnumerator SpawnAfterGeneration()
    {
        // wait until RoomManager finishes generating
        while (roomManager != null && !roomManager.IsGenerationComplete)
            yield return null;

        // Fetch rooms
        List<Room> rooms = roomManager.GetAllRooms();
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogError("GameManager: No rooms available to spawn in!");
            yield break;
        }

        // Pick distinct rooms if requested
        List<Room> available = new List<Room>(rooms);

        // Spawn Player
        Room playerRoom = PickAndRemoveRandomRoom(available);
        Vector3 playerPos = playerRoom.GetRandomSpawnPosition(roomSizeForFallback);
        playerInstance = Instantiate(Tank, playerPos, Quaternion.identity);
        playerInstance.tag = "Player"; // ensure tag is set for AI vision
        // Optionally set camera follow
        if (mainCamera == null) mainCamera = Camera.main;

        // If you’re using Cinemachine, set the virtual camera’s Follow to playerInstance.transform instead.

        // Spawn AIs
        for (int i = 0; i < aiCount; i++)
        {
            Room aiRoom = separateRooms ? PickAndRemoveRandomRoom(available, fallbackRooms: rooms) : rooms[Random.Range(0, rooms.Count)];
            if (aiRoom == null) aiRoom = rooms[Random.Range(0, rooms.Count)];

            Vector3 aiPos = aiRoom.GetRandomSpawnPosition(roomSizeForFallback);
            var ai = Instantiate(AITank, aiPos, Quaternion.identity);
            aiInstances.Add(ai);
        }
    }

    private Room PickAndRemoveRandomRoom(List<Room> pool, List<Room> fallbackRooms = null)
    {
        if (pool != null && pool.Count > 0)
        {
            int idx = Random.Range(0, pool.Count);
            Room r = pool[idx];
            pool.RemoveAt(idx);
            return r;
        }
        // fallback if pool exhausted
        if (fallbackRooms != null && fallbackRooms.Count > 0)
            return fallbackRooms[Random.Range(0, fallbackRooms.Count)];
        return null;
    }

    internal void RegisterController(Controller controller)
    {
        throw new System.NotImplementedException();
    }

    internal void UnregisterController(Controller controller)
    {
        throw new System.NotImplementedException();
    }
}


