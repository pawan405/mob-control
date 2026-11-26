

using UnityEngine;

public class Bomb_Trigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the bomb!");

            Destroy(other.gameObject);
        }
    }
}
