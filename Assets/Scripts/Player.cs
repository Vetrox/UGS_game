using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum NextMove {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    private NextMove nextMove;
    private Rigidbody rigidBody;

    [SerializeField] private Transform level;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(4.0f * Vector3.forward, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (nextMove != NextMove.NONE) {
            // don't get new move if we haven't finished yet
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < 0) {
            nextMove = NextMove.LEFT;
        } else if (horizontalInput > 0) {
            nextMove = NextMove.RIGHT;
        }

        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput < 0) {
            nextMove = NextMove.DOWN;
        } else if (verticalInput > 0) {
            nextMove = NextMove.UP;
        }
    }

    void FixedUpdate()
    {
        switch (nextMove) {
            case NextMove.LEFT:
                if (transform.position.x <= -1) {
                    nextMove = NextMove.NONE;
                    level.position = new Vector3(level.position.x - 1, 0, 0);
                    transform.position = new Vector3(0, 0, transform.position.z);
                    rigidBody.velocity = new Vector3(0, 0, 1);
                } else {
                    rigidBody.AddForce(Vector3.left, ForceMode.VelocityChange);
                }
                break;
            case NextMove.RIGHT:
                if (transform.position.x >= 1) {
                    nextMove = NextMove.NONE;
                    level.position = new Vector3(level.position.x + 1, 0, 0);
                    transform.position = new Vector3(0, 0, transform.position.z);
                    rigidBody.velocity = new Vector3(0, 0, 1);
                } else {
                    rigidBody.AddForce(Vector3.right, ForceMode.VelocityChange);
                }
                break;

            default:
                break;
        }
    }
}
