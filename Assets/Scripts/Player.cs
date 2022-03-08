using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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



    private Vector2Int lanePos = new Vector2Int(0, 1);
    private NextMove nextMove;
    private Rigidbody rigidBody;
    public Vector2 forceMult = new Vector2(10, 20);
    [Range(0f, 1f)]
    public float deadzone = 0.25f;
    private bool firstPhysicsMovement = true;
    private bool wasUnderDeadzone = true;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(0, 0, 4, ForceMode.VelocityChange);
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
        firstPhysicsMovement = true;

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
        } else if (verticalInput > 0)
        {
            lanePos.y++;
            nextMove = NextMove.UP;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (nextMove == NextMove.UP && collision.collider.CompareTag("FloorTile"))
        {
            nextMove = NextMove.NONE;
        }
        if (collision.impulse.z < -0.21f)
        {
            gameOver = true;
            Invoke("GameOver", 1);
        }
    }

    void GameOver()
    {
        // TODO: Consider pausing the physics from now on, in case the falling player cause too much CPU usage.
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    void FixedUpdate()
    {
        if (gameOver)
        {
            return;
        }
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, 4);


        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 curVel = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
        Vector2 nextPos = curPos + curVel * Time.fixedDeltaTime;

        Vector2 diff = curPos - lanePos;
        Vector2 nextDiff = nextPos - lanePos;

        bool should_reset = false; //  nextDiff.sqrMagnitude > diff.sqrMagnitude;

        if (nextMove == NextMove.RIGHT)
        {
            should_reset = nextPos.x > lanePos.x;
        } else if (nextMove == NextMove.LEFT)
        {
            should_reset = nextPos.x < lanePos.x;
        }

        if (nextMove != NextMove.NONE && should_reset)
        {
            nextMove = NextMove.NONE;
        }

        if (nextMove == NextMove.NONE)
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
            transform.position = new Vector3(lanePos.x, transform.position.y, transform.position.z);
        }

        if (firstPhysicsMovement)
        {
            firstPhysicsMovement = false;
            var moveVec = MoveToV3(nextMove);
            rigidBody.AddForce(moveVec.x * forceMult.x, moveVec.y * forceMult.y, 0, ForceMode.VelocityChange);
        }
    }
}
