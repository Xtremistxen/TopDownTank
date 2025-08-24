using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public enum GameState { Menu, Options, Play, GameOver }

    [Header("Gameplay State Objects")]
    public GameObject pressStartStateObject;
    public GameObject mainMenuStateObject;
    public GameObject playGameStateObject;
    public GameObject gameOverVictoryObject;
    public GameObject gameOverFailureObject;
    public GameObject gameOptionsObject;
    public GameObject gameCreditsObject;

    [Header("Gameplay Settings")]
    public bool isSplitScreen = false;


    // Singleton 
    public static GameManager Instance { get; private set; }

    //  Scene References 
    [Header("Scene References")]
    [SerializeField] private RoomManager roomManager; // Drag your RoomManager here (procedural gen)
    [SerializeField] private Camera mainCamera;       // Optional (for follow setups if you want)

    //  Spawn Data 
    [Header("Prefabs")]
    [SerializeField] private GameObject playerTankPrefab; // Player tank prefab
    [SerializeField] private GameObject aiTankPrefab;     // AI tank prefab

    [Header("Spawn Settings")]
    [SerializeField] private int aiCount = 3;
    [SerializeField] private bool separateRooms = true;
    [SerializeField] private Vector2 roomSizeForFallback = new Vector2(20f, 12f); // only used if room has no spawn points

    private GameObject playerInstance;
    private readonly List<GameObject> aiInstances = new();

    //  auto-updated by Controller.Start/OnDestroy
    public readonly List<Controller>       controllers       = new();
    public readonly List<PlayerController> playerControllers = new();
    public readonly List<Controller>       aiControllers     = new(); // stored as Controller (AI detected by name)

    // Unity Lifecycle 
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // DontDestroyOnLoad(gameObject); // enable if add multi-scene flow
    }

    private void Start()
    {
        // This will start our start state
        ChangeGameplayState(pressStartStateObject);
    }

    void StartGamePlay()
    {
        if (!roomManager) roomManager = FindObjectOfType<RoomManager>();
        if (!mainCamera)  mainCamera  = Camera.main;
    }

    private void ChangeGameplayState(GameObject gameplayStateObject)
    {
        DeactivateAllStates();
        gameplayStateObject.SetActive(true);
    }
    // All of our game states as inactive
    private void DeactivateAllStates()
    {

        pressStartStateObject.SetActive(false);
        mainMenuStateObject.SetActive(false);
        playGameStateObject.SetActive(false);
        gameOverVictoryObject.SetActive(false);
        gameOverFailureObject.SetActive(false);
        gameOptionsObject.SetActive(false);
        gameCreditsObject.SetActive(false);
    }

    public void ActivateMainMenu()
    {
        // Here this will change to the main menu state
        ChangeGameplayState(mainMenuStateObject);
    }

    public void ActivateGameplay()
    {
        // activates the object in scene
        ChangeGameplayState(playGameStateObject);
        //Starts the game
        SceneManager.LoadScene("Main");
    }
    public void ActivateOptionsScreen()
    {
        // activates the object in scene
        ChangeGameplayState(gameOptionsObject);
    }
    public void ActivateCreditsScreen()
    {
        // activates the object in scene
        ChangeGameplayState(gameCreditsObject);
    }

    public void ActivateVictoryScreen()
    {
        // activates the object in scene
        ChangeGameplayState(gameOverVictoryObject);
    }

    public void ActivateLoseScreen()
    {
        // activates the object in scene
        ChangeGameplayState(gameOverFailureObject);
    }

    private IEnumerator BootstrapGameplay()
    {
        // If your RoomManager auto-generates in Start, just wait for completion
        while (roomManager != null && !roomManager.IsGenerationComplete)
            yield return null;

        // Spawn player + AI
        yield return StartCoroutine(SpawnAfterGeneration());
    }

    // Spawning 
    private IEnumerator SpawnAfterGeneration()
    {
        // Get all rooms from your generator
        List<Room> rooms = roomManager.GetAllRooms();
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogError("GameManager: No rooms available to spawn in!");
            yield break;
        }

        // Clear old AI instances if restarting
        foreach (var go in aiInstances) if (go) Destroy(go);
        aiInstances.Clear();
        if (playerInstance) { Destroy(playerInstance); playerInstance = null; }

        // Build pool for distinct-room placement
        List<Room> pool = new List<Room>(rooms);

        //  Player 
        Room playerRoom = PickAndRemoveRandomRoom(pool, rooms);
        Vector3 playerPos = playerRoom.GetRandomSpawnPosition(roomSizeForFallback);
        playerInstance = Instantiate(playerTankPrefab, playerPos, Quaternion.identity);
        playerInstance.tag = "Player";

    
        //  AIs 
        for (int i = 0; i < aiCount; i++)
        {
            Room aiRoom = separateRooms ? PickAndRemoveRandomRoom(pool, rooms)
                                        : rooms[Random.Range(0, rooms.Count)];
            Vector3 aiPos = aiRoom.GetRandomSpawnPosition(roomSizeForFallback);
            var ai = Instantiate(aiTankPrefab, aiPos, Quaternion.identity);
            aiInstances.Add(ai);
        }

        yield break;
    }

    private Room PickAndRemoveRandomRoom(List<Room> pool, List<Room> fallbackRooms)
    {
        if (pool != null && pool.Count > 0)
        {
            int idx = Random.Range(0, pool.Count);
            Room r = pool[idx];
            pool.RemoveAt(idx);
            return r;
        }
        return (fallbackRooms != null && fallbackRooms.Count > 0)
            ? fallbackRooms[Random.Range(0, fallbackRooms.Count)]
            : null;
    }

    //  Registration by your existing Controller.Start/OnDestroy
    public void RegisterController(Controller c)
    {
        if (c == null) return;

        if (!controllers.Contains(c))
            controllers.Add(c);

        // PlayerControllers list
        var pc = c as PlayerController;
        if (pc != null && !playerControllers.Contains(pc))
            playerControllers.Add(pc);

        // AI Controllers list (no brittle type reference)
        // We detect AI by checking if the GameObject has a component named "AITankController"
        if (c.gameObject.GetComponent("AITankController") != null)
        {
            if (!aiControllers.Contains(c))
                aiControllers.Add(c);
        }
    }

    public void UnregisterController(Controller c)
    {
        if (c == null) return;

        controllers.Remove(c);

        var pc = c as PlayerController;
        if (pc != null) playerControllers.Remove(pc);

        if (c.gameObject.GetComponent("AITankController") != null)
            aiControllers.Remove(c);
    }

    //  Convenience 
    public RoomManager Level => roomManager;
}

