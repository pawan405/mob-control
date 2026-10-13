
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Road ki length ke along move karega
        transform.Translate(
            Vector3.forward * speed * Time.deltaTime,
            Space.World
        );
    }
}
