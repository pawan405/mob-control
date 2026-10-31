
using System.Collections.Generic;


//using System.Diagnostics;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Transform spawnPoint;

    [Header("Formation Settings")]
    [Min(0.1f)]
    public float spacing = 1f;

    [Min(1)]
    public int columns = 5;

    private readonly List<GameObject> players =
        new List<GameObject>();

    public int CurrentCount
    {
        get
        {
            players.RemoveAll(player => player == null);
            return players.Count;
        }
    }

    private void Start()
    {
        Debug.Log("TEST 1: CrowdManager Started!");

        RegisterExistingPlayers();

        Debug.Log("TEST 2: Player Count = " + CurrentCount);
    }

    private void RegisterExistingPlayers()
    {
        players.Clear();

        // Register the existing crowd members.
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                if (!players.Contains(child.gameObject))
                {
                    players.Add(child.gameObject);
                }
            }
        }
    }

    public void AddPlayers(int amount)
    {
        if (amount <= 0)
            return;

        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is not assigned!");
            return;
        }

        int startingCount = CurrentCount;

        for (int i = 0; i < amount; i++)
        {
            Vector3 position = GetSpawnPosition(
                startingCount + i
            );

            GameObject newPlayer = Instantiate(
                playerPrefab,
                position,
                playerPrefab.transform.rotation,
                transform
            );

            newPlayer.tag = "Player";

            players.Add(newPlayer);
        }

        Debug.Log(
            "Added " + amount +
            " players. Total: " + CurrentCount
        );
    }

    public void RemovePlayers(int amount)
    {
        if (amount <= 0)
            return;

        int removeCount = Mathf.Min(amount, CurrentCount);

        for (int i = 0; i < removeCount; i++)
        {
            int lastIndex = players.Count - 1;
            GameObject player = players[lastIndex];

            players.RemoveAt(lastIndex);

            if (player != null)
            {
                Destroy(player);
            }
        }

        Debug.Log(
            "Removed " + removeCount +
            " players. Total: " + CurrentCount
        );
    }

    public void MultiplyPlayers(int multiplier)
    {
        if (multiplier <= 0)
            return;

        int oldCount = CurrentCount;
        int targetCount = oldCount * multiplier;

        AddPlayers(targetCount - oldCount);
    }

    public void DividePlayers(int divisor)
    {
        if (divisor <= 0)
            return;

        int oldCount = CurrentCount;
        int targetCount = oldCount / divisor;

        RemovePlayers(oldCount - targetCount);
    }

    private Vector3 GetSpawnPosition(int index)
    {
        Vector3 origin = spawnPoint != null
            ? spawnPoint.position
            : transform.position;

        int safeColumns = Mathf.Max(1, columns);

        int row = index / safeColumns;
        int column = index % safeColumns;

        float xOffset =
            (column - (safeColumns - 1) / 2f) * spacing;

        // Place additional rows behind the existing formation.
        float zOffset = -row * spacing;

        return origin + new Vector3(
            xOffset,
            0f,
            zOffset
        );
    }
}
