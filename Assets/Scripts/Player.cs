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

    public Vector2 forceMult = new Vector2(2, 5);
    [Range(0f, 1f)]
    public float deadzone = 0.25f;
    public float maxSlideDuration = 0.5f;

    private Vector2Int lanePos = new Vector2Int(0, 1);
    private NextMove nextMove;
    private float slideStart;
    private Rigidbody rigidBody;
    private bool firstPhysicsMovement = true;
    private bool wasUnderDeadzone = true;
    private bool gameOver = false;


    private float forwardForce = 4;
    [SerializeField] private float forwardForceMult = 0.05f;
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

            default:
                return Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        forwardForce = 5.0f * GameManager.GetCurrentLevel().bpm / 60.0f;
        rigidBody.AddForce(0, 0, forwardForce, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool underDeadzone = Mathf.Abs(horizontalInput) < deadzone && Mathf.Abs(verticalInput) < deadzone;

        if (underDeadzone) {
            wasUnderDeadzone = true;
            return;
        }

        if (!wasUnderDeadzone || nextMove != NextMove.NONE) {
            return;
        }

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
        } else if (verticalInput > 0)
        {
            nextMove = NextMove.UP;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (nextMove == NextMove.UP && collision.collider.CompareTag("FloorTile"))
        {
            nextMove = NextMove.NONE;
            firstPhysicsMovement = true;
        }
        if (collision.impulse.z < -0.21f)
        {
            Invoke("GameOver", 1);
        }
    }

    void GameOver()
    {
        gameOver = true;
        GameManager.StopCurrentSong();
        // TODO: Consider pausing the physics from now on, in case the falling player cause too much CPU usage.
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    void FixedUpdate()
    {
        if (gameOver) return;
        if (transform.position.y < -1)
        {
            GameOver();
            return;
        }
        if (nextMove == NextMove.NONE) return;
        

        if (firstPhysicsMovement)
        {
            firstPhysicsMovement = false;
            if (nextMove == NextMove.DOWN) {
                transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.right);
                slideStart = Time.realtimeSinceStartup;
            } else {
                var moveVec = MoveToV3(nextMove);
                //rigidBody.velocity = 5.0f * Vector3.forward;
                rigidBody.AddForce(moveVec.x * forceMult.x, moveVec.y * forceMult.y, 0, ForceMode.VelocityChange);
            }

            return;
        }

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, forwardForce);

        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 curVel = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
        Vector2 nextPos = curPos + curVel * Time.fixedDeltaTime;

        Vector2 diff = curPos - lanePos;
        Vector2 nextDiff = nextPos - lanePos;

        bool should_reset = false; //  nextDiff.sqrMagnitude > diff.sqrMagnitude;

        switch (nextMove) {
            case NextMove.RIGHT:
                should_reset = nextPos.x > lanePos.x;
                break;
            case NextMove.LEFT:
                should_reset = nextPos.x < lanePos.x;
                break;
            case NextMove.DOWN:
                should_reset = Time.realtimeSinceStartup > slideStart + maxSlideDuration;
                break;
        }

        if (should_reset) {
            nextMove = NextMove.NONE;
            firstPhysicsMovement = true;
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
            transform.position = new Vector3(lanePos.x, transform.position.y, transform.position.z);
            transform.rotation = Quaternion.identity;
        }
    }
}