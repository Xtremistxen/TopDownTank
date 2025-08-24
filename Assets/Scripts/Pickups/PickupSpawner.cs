using System.Collections;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Pickup Prefab")]
    public GameObject pickupPrefab;

    [Header("Respawn Settings")]
    public bool spawnOnStart = true;
    public float respawnDelay = 10f;

    private GameObject currentPickup;
    private bool respawning;

    void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void Spawn()
    {
        if (pickupPrefab == null || currentPickup != null) return;
        currentPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity, transform);
    }

    public void HandleCollected()
    {
        currentPickup = null;
        if (!respawning)
            StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        respawning = true;
        yield return new WaitForSeconds(respawnDelay);
        Spawn();
        respawning = false;
    }
}

