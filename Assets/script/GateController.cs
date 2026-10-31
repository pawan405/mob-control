

using UnityEngine;

public class GateController : MonoBehaviour
{
    public enum GateType
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    [Header("Gate Settings")]
    public GateType gateType = GateType.Add;

    public int value = 5;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TEST 3: Gate touched by " + other.name);

        // Ignore further contacts after this gate is activated.
        if (activated)
            return;

        // Find the CrowdManager in the player's parents.
        CrowdManager crowd =
            other.GetComponentInParent<CrowdManager>();

        if (crowd == null)
        {
            Debug.LogWarning(
                "TEST 4: CrowdManager not found for " + other.name
            );
            return;
        }

        activated = true;

        Debug.Log("TEST 4: CrowdManager found!");

        switch (gateType)
        {
            case GateType.Add:
                crowd.AddPlayers(value);
                break;

            case GateType.Subtract:
                crowd.RemovePlayers(value);
                break;

            case GateType.Multiply:
                crowd.MultiplyPlayers(value);
                break;

            case GateType.Divide:
                crowd.DividePlayers(value);
                break;
        }

        Debug.Log(
            "TEST 5: Gate processed. Total players = "
            + crowd.CurrentCount
        );
    }
}
