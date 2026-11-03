

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

    [Min(1)]
    public int value = 5;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        CrowdManager crowd =
            other.GetComponentInParent<CrowdManager>();

        if (crowd == null)
            return;

        activated = true;

        int before = crowd.CurrentCount;

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
            "Gate: " + gateType +
            " | Before: " + before +
            " | After: " + crowd.CurrentCount
        );
    }
}
