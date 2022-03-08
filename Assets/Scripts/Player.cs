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
                return Vector3.left + Vector3.forward;
            case NextMove.RIGHT:
                return Vector3.right + Vector3.forward;
            case NextMove.UP:
                return Vector3.up + Vector3.forward;
            case NextMove.DOWN:
                return Vector3.down + Vector3.forward;
            default:
                return Vector3.zero + Vector3.forward;
        }
    }



    private Vector2Int lanePos = new Vector2Int(0, 1);
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
        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 curVel = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
        Vector2 nextPos = curPos + curVel * Time.fixedDeltaTime;

        Vector2 diff = curPos - lanePos;
        Vector2 nextDiff = nextPos - lanePos;

        bool should_reset = nextDiff.sqrMagnitude > diff.sqrMagnitude;
        if (nextMove != NextMove.NONE && should_reset)
        {
            nextMove = NextMove.NONE;
        }

        if (nextMove == NextMove.NONE)
        {
            transform.position = new Vector3(lanePos.x, lanePos.y, transform.position.z);
        }

        rigidBody.velocity = MoveToV3(nextMove) * horizontalForceMult;
    }
}
