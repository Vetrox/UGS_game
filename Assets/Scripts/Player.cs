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

    public float jumpHeight = 2.0f, jumpDistance = 2.0f;
    [Range(0f, 1f)]
    public float deadzone = 0.25f;
    public float duckDuration = 0.5f;

    private float shootingCooldown;
    public float shootingDelay;
    public Transform bulletPrefab;

    private AudioSource laserShootSound;

    private Vector2Int lanePos = new Vector2Int(0, 1);
    private NextMove nextMove;
    private float duckStart;

    private Rigidbody rigidBody;
    private Animator animator;
    private Animator barrelAnimator;
    private SphereCollider sphereCollider;

    private bool firstPhysicsMovement = true;
    private bool wasUnderDeadzone = true;
   
    private float forwardVelocity;
    private float sidewardVelocity;
    private float jumpVelocity;

    Vector3 MoveToV3(NextMove move)
    {
        switch(move)
        {
            case NextMove.LEFT:
                return Vector3.left * sidewardVelocity + Vector3.forward * forwardVelocity;
            case NextMove.RIGHT:
                return Vector3.right * sidewardVelocity + Vector3.forward * forwardVelocity;
            case NextMove.UP:
                return Vector3.up * jumpVelocity + Vector3.forward * forwardVelocity;

            default:
                return Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        barrelAnimator = GetComponentsInChildren<Animator>()[1]; // skip the parent
        sphereCollider = GetComponent<SphereCollider>();
        laserShootSound = GetComponent<AudioSource>();

        // forward velocity from bpm
        float beatLength = 60.0f / GameManager.GetCurrentLevel().bpm;
        forwardVelocity = 5.0f / beatLength;
        rigidBody.velocity = Vector3.forward * forwardVelocity;

        // sidewards velocity from bpm (should take 1/8 beatLength)
        sidewardVelocity = 4.0f / beatLength;

        // gravity and upwards velocity from bpm
        // i have no idea how or why this formula works, but it does, so don't touch it
        float t = jumpDistance / forwardVelocity / 2.0f;
        float gravity = 2.0f * jumpHeight / (t * t);
        jumpVelocity = gravity * t;
        Physics.gravity = Vector3.down * gravity;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        shootingCooldown = Time.realtimeSinceStartup + shootingDelay;
        barrelAnimator.SetTrigger("Shoot");
        laserShootSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameOver) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsPaused()) GameManager.ResumeLevel();
            else
            {
                GameManager.PauseLevel();
                return;
            }
        }

        // shooting happens independently of movement, albeit with a certain cooldown period
        if (Time.realtimeSinceStartup > shootingCooldown && Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
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
        if (nextMove == NextMove.UP && collision.collider.CompareTag("Level"))
        {
            nextMove = NextMove.NONE;
            firstPhysicsMovement = true;
        }
        if (!GameManager.gameOver && (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Destructible")))
        {
            rigidBody.velocity = Vector3.zero;
            print("Collided with obstacle");
            GameOver();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(!GameManager.gameOver && collider.CompareTag("Goal"))
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
        SavePercentage();
        GameManager.gameOver = true;
        Invoke("LoadYouWonScreen", 0.5f);
    }
    
    void SavePercentage()
    { // a combination of serialization and bad api lead to this junk of code.
        var highScores = GameManager.PersistantSettings.Instance().highScores.ToArray();
        for (int i = 0; i < highScores.Length; ++i)
        {
            if (highScores[i].id.Equals(GameManager.GetCurrentLevel().id))
            {
                if (highScores[i].percentage < GameManager.GetCurrentLevelPercentage())
                {
                    highScores[i].percentage = GameManager.GetCurrentLevelPercentage();
                }
                GameManager.PersistantSettings.Instance().highScores = new List<GameManager.Pair>(highScores);
                return;
            }
        }

        GameManager.PersistantSettings.Instance().highScores.Add(
            new GameManager.Pair(GameManager.GetCurrentLevel().id,
            GameManager.GetCurrentLevelPercentage()
        ));
    }

    void GameOver()
    {
        SavePercentage();
        GameManager.gameOver = true;
        rigidBody.velocity = new Vector3(0.0f, rigidBody.velocity.y, rigidBody.velocity.z);
        Invoke("LoadGameOverScreen", 1);
    }

    void FixedUpdate()
    {
        if (!GameManager.gameOver && (transform.position.y < -1 || rigidBody.velocity.z < forwardVelocity * 0.9)) {
            print(rigidBody.velocity.z + " expected " + forwardVelocity);
            GameOver();
            return;
        }

        if (GameManager.gameOver) return;

        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, forwardVelocity);
        if (nextMove == NextMove.NONE) return;
        
        if (firstPhysicsMovement)
        {
            firstPhysicsMovement = false;
            if (nextMove == NextMove.DOWN) {
                animator.SetTrigger("DuckEntry");
                sphereCollider.radius = 0.352f;
                sphereCollider.center = new Vector3(0.0f, 0.352f, 0.0f);
                duckStart = Time.realtimeSinceStartup;
            } else {
                var moveVec = MoveToV3(nextMove);
                rigidBody.velocity = moveVec;
            }
            return;
        }

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
                should_reset = Time.realtimeSinceStartup > duckStart + duckDuration;
                if (should_reset) {
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