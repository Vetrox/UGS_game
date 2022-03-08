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

    private NextMove nextMove = NextMove.NONE;
    private bool isMoving = false;
    private float timeSinceLastMove = 0.0f; // seconds

    private Rigidbody rigidBody;

    public Transform level;
    public float forwardVelocity = 1.0f;
    public float moveTime = 0.25f;
    public float laneSeparation = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(forwardVelocity * Vector3.forward, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving || Time.realtimeSinceStartup < timeSinceLastMove + 0.1f) {
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
        nextMove = NextMove.NONE;
        isMoving = false;
        timeSinceLastMove = Time.realtimeSinceStartup;

        rigidBody.velocity = Vector3.forward;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        switch (nextMove) {
            case NextMove.LEFT:
                if (!isMoving) {
                    isMoving = true;
                    rigidBody.AddForce(laneSeparation / moveTime * Vector3.left, ForceMode.VelocityChange);
                } else {
                    if (transform.position.x + rigidBody.velocity.x * Time.fixedDeltaTime <= -laneSeparation) {
                        level.position = new Vector3(level.position.x + laneSeparation, 0, 0);
                        EndMove();
                    }
                }
                break;
            case NextMove.RIGHT:
                if (!isMoving) {
                    isMoving = true;
                    rigidBody.AddForce(laneSeparation / moveTime * Vector3.right, ForceMode.VelocityChange);
                } else {
                    if (transform.position.x + rigidBody.velocity.x * Time.fixedDeltaTime >= laneSeparation) {
                        level.position = new Vector3(level.position.x - laneSeparation, 0, 0);
                        EndMove();
                    }
                }
                break;

            default:
                break;
        }
    }
}
