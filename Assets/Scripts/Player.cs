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
    public float maxDuckDuration = 0.5f;

    private Vector2Int lanePos = new Vector2Int(0, 1);
    private NextMove nextMove;
    private float duckStart;

    private Rigidbody rigidBody;
    private Animator animator;
    private SphereCollider sphereCollider;

    private bool firstPhysicsMovement = true;
    private bool wasUnderDeadzone = true;
   
    private float forwardVelocity;

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
        animator = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();

        GameManager.gameOver = false;
        float beatLength = 60.0f / GameManager.GetCurrentLevel().bpm;
        forwardVelocity = 5.0f / beatLength;
        rigidBody.velocity = Vector3.forward * forwardVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsPaused()) GameManager.ResumeLevel();
            else
            {
                GameManager.PauseLevel();
                return;
            }
        }

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
        if (nextMove == NextMove.UP && collision.collider.CompareTag("FloorTile")) {
            nextMove = NextMove.NONE;
            firstPhysicsMovement = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Saw")) {
            rigidBody.velocity = Vector3.zero;
            print("Collided with saw");
            GameOver();
        } else if(collider.CompareTag("Goal"))
        {
            YouWon();
        }
    }

    void LoadGameOverScreen()
    {
        GameManager.StopCurrentSong();
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    void LoadYouWonScreen()
    {
        GameManager.StopCurrentSong();
        SceneManager.LoadScene("YouWon", LoadSceneMode.Additive);
    }
    
    void YouWon()
    {
        // TODO: save progress immediately here
        GameManager.gameOver = true;
        Invoke("LoadYouWonScreen", 0.5f);
    }

    void GameOver()
    {
        GameManager.gameOver = true;
        Invoke("LoadGameOverScreen", 1);
    }

    void FixedUpdate()
    {
        if (!GameManager.gameOver && (transform.position.y < -1 || rigidBody.velocity.z < forwardVelocity * 0.9)) {
            print(rigidBody.velocity.z + " expected " + forwardVelocity);
            GameOver();
            return;
        }

        if (GameManager.gameOver || nextMove == NextMove.NONE) {
            return;
        }
        
        if (firstPhysicsMovement)
        {
            firstPhysicsMovement = false;
            if (nextMove == NextMove.DOWN) {
                print("DUCK ENTRY!");
                animator.SetTrigger("DuckEntry");
                sphereCollider.radius = 0.352f;
                sphereCollider.center = new Vector3(0.0f, 0.352f, 0.0f);
                duckStart = Time.realtimeSinceStartup;
            } else {
                var moveVec = MoveToV3(nextMove);
                rigidBody.velocity = new Vector3(moveVec.x * forceMult.x, moveVec.y * forceMult.y, forwardVelocity);
            }
            return;
        }

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, forwardVelocity);

        Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 curVel = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
        Vector2 nextPos = curPos + curVel * Time.fixedDeltaTime;

        bool should_reset = false;
        switch (nextMove) {
            case NextMove.RIGHT:
                should_reset = nextPos.x > lanePos.x;
                break;
            case NextMove.LEFT:
                should_reset = nextPos.x < lanePos.x;
                break;
            case NextMove.DOWN:
                should_reset = Time.realtimeSinceStartup > duckStart + maxDuckDuration;
                if (should_reset) {
                    print("DUCK EXIT!");
                    animator.SetTrigger("DuckExit");
                    sphereCollider.radius = 0.5f;
                    sphereCollider.center = new Vector3(0.0f, 0.5f, 0f);
                }
                break;
        }

        if (should_reset) {
            nextMove = NextMove.NONE;
            firstPhysicsMovement = true;
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, forwardVelocity);
            transform.position = new Vector3(lanePos.x, transform.position.y, transform.position.z);
            transform.rotation = Quaternion.identity;
        }
    }
}