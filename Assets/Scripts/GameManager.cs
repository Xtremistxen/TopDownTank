using System.Collections.Generic;
using UnityEngine;

// The Gamemanager has been set to a Singleton, that manages gloval game states.
// Also including references to the current level and lists of the active controllers and pawns

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // The singleton

    public Level currentLevel; // The reference to the current level

    // Below will list all of the active controllers and pawns which include the player and AI controllers
    public List<Controller> allControllers = new List<Controller>();
    public List<PlayerController> playerControllers = new List<PlayerController>();
    public List<Pawn> allPawns = new List<Pawn>();

    void Awake() // This will enforce the singleton pattern
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // So it persistst across all scenes
    }

    public void SetLevel(Level level) //sets an active level being called from the level script
    {
        currentLevel = level;
    }

    public void RegisterController(Controller controller) // Add a controller to the master list, player list and player controllers
    {
        if (!allControllers.Contains(controller))
            allControllers.Add(controller);

        if (controller is PlayerController player)
        {
            if (!playerControllers.Contains(player))
                playerControllers.Add(player);
        }
    }

    public void UnregisterController(Controller controller) // Removes the controller from relevant lists
    {
        allControllers.Remove(controller);
        if (controller is PlayerController player)
        {
            playerControllers.Remove(player);
        }
    }

    public void RegisterPawn(Pawn pawn) // Adds a pawn to the global list
    {
        if (!allPawns.Contains(pawn))
            allPawns.Add(pawn);
    }

    public void UnregisterPawn(Pawn pawn)
    {
        allPawns.Remove(pawn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) //Prints a debug ensuring the list works
        {
            Debug.Log($"Controllers: {allControllers.Count}, Players: {playerControllers.Count}, Pawns: {allPawns.Count}");
        }
    }
}

