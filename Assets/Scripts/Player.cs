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
        DOWN,
    }

    private NextMove nextMove;
    private bool isMoving;
    private Rigidbody rigidBody;
    private float timeSinceLastMove; // seconds

    [SerializeField] private Transform level;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        nextMove = NextMove.NONE;
        timeSinceLastMove = 0.0f;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(Vector3.forward, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving || Time.realtimeSinceStartup < timeSinceLastMove + 0.2f) {
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

    private void EndMove()
    {
        timeSinceLastMove = Time.realtimeSinceStartup;
        rigidBody.velocity = Vector3.forward;
        transform.position = new Vector3(0, 2, transform.position.z);
        isMoving = false;
        nextMove = NextMove.NONE;
    }

    void FixedUpdate()
    {
        switch (nextMove) {
            case NextMove.LEFT:
                if (!isMoving) {
                    isMoving = true;
                    rigidBody.AddForce(5 * Vector3.left, ForceMode.VelocityChange);
                } else {
                    if (transform.position.x <= -1) {
                        level.position = new Vector3(level.position.x + 1, 0, 0);
                        EndMove();
                    }
                }
                break;
            case NextMove.RIGHT:
                if (!isMoving) {
                    isMoving = true;
                    rigidBody.AddForce(5 * Vector3.right, ForceMode.VelocityChange);
                } else {
                    if (transform.position.x >= 1) {
                        level.position = new Vector3(level.position.x - 1, 0, 0);
                        EndMove();
                    }
                }
                break;

            default:
                break;
        }
    }
}
