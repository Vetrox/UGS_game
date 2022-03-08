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



    private Vector2 lanePos = new Vector2(0, 0);
    private NextMove nextMove;
    private Rigidbody rigidBody;
    public float horizontalForceMult = 10;
    [Range(0f, 1f)]
    public float deadzone = 0.25f;

    private bool wasUnderDeadzone = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(4.0f * Vector3.forward, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool under_deadzone = Mathf.Abs(horizontalInput) < deadzone && Mathf.Abs(verticalInput) < deadzone;
        if (under_deadzone)
        {
            wasUnderDeadzone = true;
            return;
        }

        if (!wasUnderDeadzone || nextMove != NextMove.NONE) return;
        wasUnderDeadzone = false;

        if (horizontalInput < 0) {
            nextMove = NextMove.LEFT;
            lanePos.x--;
        } else if (horizontalInput > 0) {
            nextMove = NextMove.RIGHT;
            lanePos.x++;
        } else if (verticalInput < 0)
        {
            nextMove = NextMove.DOWN;
            lanePos.y--;
        }
        else if (verticalInput > 0)
        {
            lanePos.y++;
            nextMove = NextMove.UP;
        }

    }

    void FixedUpdate()
    {

        float nextX = transform.position.x + rigidBody.velocity.x * Time.fixedDeltaTime;

        float diff = Mathf.Abs(transform.position.x - lanePos.x);
        float nextDiff = Mathf.Abs(nextX - lanePos.x);
        bool should_reset = diff < nextDiff;

        if (nextMove != NextMove.NONE && should_reset)
        {
            nextMove = NextMove.NONE;
        }

        if (nextMove == NextMove.NONE)
        {
            transform.position = new Vector3(lanePos.x, transform.position.y, transform.position.z);
        }

        rigidBody.velocity = MoveToV3(nextMove) * horizontalForceMult;
    }
}
