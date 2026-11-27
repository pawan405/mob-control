
using UnityEngine;

public class HammerDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageCount = 5;

    private void OnTriggerEnter(Collider other)
    {
        CrowdManager crowd = other.GetComponentInParent<CrowdManager>();

        if (crowd != null)
        {
            crowd.RemovePlayers(damageCount);
        }
    }
}
