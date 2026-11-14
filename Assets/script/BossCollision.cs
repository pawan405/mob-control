

using UnityEngine;

public class BossCollision : MonoBehaviour
{
    [Header("Boss Settings")]
    public int requiredPlayers = 7;

    private bool defeated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (defeated)
            return;

        PlayerControl player =
            other.GetComponentInParent<PlayerControl>();

        if (player == null)
            return;

        Transform playerRoot = player.transform;

        // Count active player characters
        int playerCount = 0;

        foreach (Transform child in playerRoot)
        {
            if (child.gameObject.activeInHierarchy)
                playerCount++;
        }

        Debug.Log("Player Count: " + playerCount);
        Debug.Log("Required Players: " + requiredPlayers);

        if (playerCount >= requiredPlayers)
        {
            // Player wins
            defeated = true;

            int remainingPlayers =
                playerCount - requiredPlayers;

            // Remove players used to defeat boss
            int removed = 0;

            for (int i = playerRoot.childCount - 1;
                 i >= 0 && removed < requiredPlayers; i--)
            {
                Transform child = playerRoot.GetChild(i);

                if (child.gameObject.activeInHierarchy)
                {
                    Destroy(child.gameObject);
                    removed++;
                }
            }

            Debug.Log("BOSS DEFEATED!");
            Debug.Log("Remaining Players: " + remainingPlayers);

            Destroy(gameObject, 1f);
        }
        else
        {
            // Player loses
            Debug.Log("PLAYER DEFEATED!");
            player.enabled = false;

            // Stop player movement
            Debug.Log("GAME OVER!");
        }
    }
}
