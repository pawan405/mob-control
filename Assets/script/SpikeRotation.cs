
using UnityEngine;

public class SpikeRotation : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    private float initialY;
    private float initialZ;

    void Start()
    {
        initialY = transform.position.y;
        initialZ = transform.position.z;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);

        float x = Mathf.Lerp(
            pointA.position.x,
            pointB.position.x,
            t
        );

        transform.position = new Vector3(
            x,
            initialY,
            initialZ
        );
    }
}
