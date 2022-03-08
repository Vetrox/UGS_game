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

    Vector3 MoveToV3(NextMove move)
    {
        switch(move)
        {
            case NextMove.LEFT:
                return Vector3.left;
            case NextMove.RIGHT:
                return Vector3.right;
            case NextMove.UP:
                return Vector3.up;
            case NextMove.DOWN:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }


    private NextMove nextMove;
    private Rigidbody rigidBody;

    private int currentLaneIndex = 0;
    public int laneWidth = 5;
    public float horizontalForceMult = 10;

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
            currentLaneIndex--;
        } else if (horizontalInput > 0) {
            nextMove = NextMove.RIGHT;
            currentLaneIndex++;
        }

        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput < 0)
        {
            nextMove = NextMove.DOWN;
        }
        else if (verticalInput > 0)
        {
            nextMove = NextMove.UP;
        }

    }

    void FixedUpdate()
    {
        int laneX = currentLaneIndex * laneWidth;
        bool should_reset = false;
        if (nextMove == NextMove.RIGHT)
        {
            should_reset = transform.position.x > laneX;
        } else if (nextMove == NextMove.LEFT)
        {
            should_reset = transform.position.x < laneX;
        }

        if (nextMove != NextMove.NONE && should_reset)
        {
            nextMove = NextMove.NONE;
        }
        if (nextMove == NextMove.NONE)
        {
            transform.position = new Vector3(laneX, transform.position.y, transform.position.z);
        }

        rigidBody.velocity = MoveToV3(nextMove) * horizontalForceMult;
    }
}
