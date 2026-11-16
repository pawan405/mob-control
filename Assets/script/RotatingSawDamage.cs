

using UnityEngine;

public class RotatingSawDamage : MonoBehaviour
{
    private bool alreadyHit = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SAW COLLISION: " + other.name);

        PlayerControl player =
            other.GetComponentInParent<PlayerControl>();

        if (player == null)
        {
            Debug.Log("PlayerControl not found!");
            return;
        }

        if (alreadyHit)
            return;

        Transform playerRoot = player.transform;

        // Find one active player character
        for (int i = playerRoot.childCount - 1; i >= 0; i--)
        {
            Transform character = playerRoot.GetChild(i);

            if (!character.gameObject.activeInHierarchy)
                continue;

            Debug.Log("Destroying: " + character.name);

            character.gameObject.SetActive(false);

            alreadyHit = true;

            if (playerRoot.childCount == 0)
            {
                player.enabled = false;
            }

            return;
        }

        Debug.Log("No player characters found!");
    }
}
