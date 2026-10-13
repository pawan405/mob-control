
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;
    public float moveSpeed = 5f;

    void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                Debug.Log("Clicked: " + hit.collider.name);

                if (hit.collider.transform == transform ||
                    hit.collider.transform.IsChildOf(transform))
                {
                    SpawnPlayer();
                }
            }
        }
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Assign Player Prefab and Spawn Point!");
            return;
        }

        GameObject player = Instantiate(
            playerPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        MobMovement movement = player.GetComponent<MobMovement>();

        if (movement == null)
            movement = player.AddComponent<MobMovement>();

        movement.speed = moveSpeed;

        Debug.Log("PLAYER SPAWNED!");
    }
}
