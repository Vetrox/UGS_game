using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float maxTravel;

    private Rigidbody rigidBody;
    private float zStart;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0, 0, bulletSpeed);
        zStart = transform.position.z;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Destructible")) {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float z = transform.position.z;
        if (z - zStart > maxTravel) {
            Destroy(gameObject);
        }
    }
}
