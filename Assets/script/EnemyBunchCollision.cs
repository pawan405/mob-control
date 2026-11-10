
using System.Collections.Generic;

using UnityEngine;

public class EnemyBunchCollision : MonoBehaviour
{
    private bool hasCollided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided)
            return;

        // Find PlayerControl on the collided object or its parent
        PlayerControl player = other.GetComponentInParent<PlayerControl>();

        if (player == null)
            return;

        hasCollided = true;

        Transform playerRoot = player.transform;
        Transform enemyRoot = transform;

        // Get all spawned player characters
        List<Transform> players = new List<Transform>();

        foreach (Transform child in playerRoot)
        {
            players.Add(child);
        }

        // Get all enemy characters
        List<Transform> enemies = new List<Transform>();

        foreach (Transform child in enemyRoot)
        {
            enemies.Add(child);
        }

        int playerCount = players.Count;
        int enemyCount = enemies.Count;

        Debug.Log("Players: " + playerCount);
        Debug.Log("Enemies: " + enemyCount);

        int destroyCount = Mathf.Min(playerCount, enemyCount);

        // Destroy equal number from both groups
        for (int i = 0; i < destroyCount; i++)
        {
            Destroy(players[i].gameObject);
            Destroy(enemies[i].gameObject);
        }

        Debug.Log("Remaining Players: " +
                  (playerCount - destroyCount));

        Debug.Log("Remaining Enemies: " +
                  (enemyCount - destroyCount));

        // Prevent repeated collision
        if (playerCount <= enemyCount)
        {
            player.enabled = false;

            if (playerCount == enemyCount)
                Debug.Log("Both groups eliminated!");
            else
                Debug.Log("Player group eliminated!");
        }
    }
}
