

using System.Collections.Generic;

using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab;

    [Header("Blob Formation Settings")]
    [Min(0.1f)]
    public float spacing = 0.8f;

    [Min(0.1f)]
    public float minimumRadius = 0.9f;

    [Min(0.01f)]
    public float blendSpeed = 8f;

    [Min(1)]
    public int placementAttempts = 100;

    private readonly List<GameObject> players =
        new List<GameObject>();

    private readonly Dictionary<GameObject, Vector3> targets =
        new Dictionary<GameObject, Vector3>();

    private readonly Dictionary<GameObject, Vector3> velocities =
        new Dictionary<GameObject, Vector3>();

    public int CurrentCount
    {
        get
        {
            players.RemoveAll(p => p == null);
            return players.Count;
        }
    }

    private void Start()
    {
        RegisterExistingPlayers();
        ArrangeFormation();

        Debug.Log("Blob Crowd Started: " + CurrentCount);
    }

    private void Update()
    {
        MovePlayersIntoFormation();
    }

    private void RegisterExistingPlayers()
    {
        players.Clear();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                players.Add(child.gameObject);
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

        for (int i = 0; i < amount; i++)
        {
            GameObject newPlayer = Instantiate(
                playerPrefab,
                transform
            );

            newPlayer.tag = "Player";

            // Start near the center of the crowd.
            newPlayer.transform.localPosition =
                Random.insideUnitSphere * 0.1f;

            players.Add(newPlayer);
        }

        ArrangeFormation();

        Debug.Log("Players after adding: " + CurrentCount);
    }

    public void RemovePlayers(int amount)
    {
        if (amount <= 0)
            return;

        int removeCount = Mathf.Min(amount, CurrentCount);

        for (int i = 0; i < removeCount; i++)
        {
            int index = players.Count - 1;
            GameObject player = players[index];

            players.RemoveAt(index);

            if (player != null)
            {
                targets.Remove(player);
                velocities.Remove(player);
                Destroy(player);
            }
        }

        ArrangeFormation();

        Debug.Log("Players after removing: " + CurrentCount);
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

    public void ArrangeFormation()
    {
        players.RemoveAll(p => p == null);

        targets.Clear();
        velocities.Clear();

        int count = CurrentCount;

        if (count == 0)
            return;

        // Crowd grows gradually as members increase.
        float radius = Mathf.Max(
            minimumRadius,
            spacing * Mathf.Sqrt(count / Mathf.PI) * 1.25f
        );

        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            Vector3 position = FindFreePosition(
                radius,
                usedPositions
            );

            usedPositions.Add(position);

            GameObject player = players[i];

            targets[player] = position;
            velocities[player] = Vector3.zero;
        }
    }

    private Vector3 FindFreePosition(
        float radius,
        List<Vector3> usedPositions)
    {
        Vector3 candidate = Vector3.zero;

        for (int attempt = 0;
             attempt < placementAttempts;
             attempt++)
        {
            Vector2 randomPoint =
                Random.insideUnitCircle * radius;

            candidate = new Vector3(
                randomPoint.x,
                0f,
                randomPoint.y
            );

            bool positionAvailable = true;

            foreach (Vector3 used in usedPositions)
            {
                if (Vector3.Distance(candidate, used) < spacing)
                {
                    positionAvailable = false;
                    break;
                }
            }

            if (positionAvailable)
                return candidate;
        }

        // Fallback if no free position was found.
        float angle = Random.Range(0f, Mathf.PI * 2f);

        float fallbackRadius = radius *
            Mathf.Sqrt(Random.value);

        return new Vector3(
            Mathf.Cos(angle) * fallbackRadius,
            0f,
            Mathf.Sin(angle) * fallbackRadius
        );
    }

    private void MovePlayersIntoFormation()
    {
        foreach (GameObject player in players)
        {
            if (player == null || !targets.ContainsKey(player))
                continue;

            Transform playerTransform = player.transform;

            Vector3 target = targets[player];

            Vector3 velocity = velocities[player];

            playerTransform.localPosition =
                Vector3.SmoothDamp(
                    playerTransform.localPosition,
                    target,
                    ref velocity,
                    1f / blendSpeed
                );

            velocities[player] = velocity;
        }
    }
}
