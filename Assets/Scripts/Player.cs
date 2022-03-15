using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    enum Move {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    // exported to editor
    public float jumpHeight = 2.0f, jumpDistance = 2.0f;
    [Range(0.0f, 1.0f)]
    public float deadzone = 0.25f;
    public float duckDuration = 0.5f;
    public float shootingDelay;
    public Transform bulletPrefab;

    // movement state
    private float shootingCooldown;
    private float duckStart;
    private Vector2Int lanePosition = new Vector2Int(0, 1);
    private Move currentMove;
    private bool acceptInput;

    // physics (we only care about xy velocity as the z position is directly set each frame)
    private Vector2 velocity;
    private Vector2 acceleration;
    private float levelTime;

    // components and resources
    private Animator animator;
    private Animator barrelAnimator;
    private SphereCollider sphereCollider;
    private AudioSource laserShootSound;

    // precalculated and preset values
    private float forwardVelocity;
    private float sidewardVelocity;
    private float jumpVelocity;
    private float gravity;
    private float levelDuration;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        barrelAnimator = GetComponentsInChildren<Animator>()[1]; // skip the parent
        sphereCollider = GetComponent<SphereCollider>();
        laserShootSound = GetComponent<AudioSource>();

        levelDuration = GameManager.GetCurrentAudioSource().clip.length;

        // forward velocity from bpm
        float beatLength = 60.0f / GameManager.GetCurrentLevel().bpm;
        forwardVelocity = 5.0f / beatLength;

        // sidewards velocity from bpm (should take 1/8 beatLength)
        sidewardVelocity = 4.0f / beatLength;

        // gravity and upwards velocity from bpm
        // i have no idea how or why this formula works, but it does, so don't touch it
        float t = jumpDistance / forwardVelocity / 2.0f;
        gravity = 2.0f * jumpHeight / (t * t);
        jumpVelocity = gravity * t;
        acceleration = Vector3.down * gravity;
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
        // game over and pause
        if (GameManager.gameOver)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.IsPaused()) { 
                GameManager.ResumeLevel();
            } else {
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
        bool newInput = Mathf.Abs(horizontalInput) >= deadzone || Mathf.Abs(verticalInput) >= deadzone;

        if (!newInput && !acceptInput) {
            // if we didn't accept input before and there is no new input, allow new input next frame
            acceptInput = true;
        }

        if (acceptInput && newInput && currentMove == Move.NONE) {
            // if we accept input and there is new input and we are not moving and we are on ground, handle it

            // disallow input next frame and handle current input
            acceptInput = false;
            if (horizontalInput < -deadzone) {
                currentMove = Move.LEFT;
                lanePosition.x--;
                // left (set leftwards velocity)
                velocity = Vector3.left * sidewardVelocity;
            } else if (horizontalInput > deadzone) {
                currentMove = Move.RIGHT;
                lanePosition.x++;
                // right (set rightwards velocity)
                velocity = Vector3.right * sidewardVelocity;
            } else if (verticalInput < -deadzone) {
                currentMove = Move.DOWN;
                // duck (change hitbox and start timer)
                animator.SetTrigger("DuckEntry");
                sphereCollider.radius = 0.352f;
                sphereCollider.center = new Vector3(0.0f, 0.352f, 0.0f);
                duckStart = Time.realtimeSinceStartup;
            } else if (verticalInput > deadzone) {
                currentMove = Move.UP;
                // jump (set upwards velocity)
                velocity = Vector3.up * jumpVelocity;
            }
        }

        // calculate next position and velocity
        Vector2 nextPosition = new Vector2(transform.position.x, transform.position.y) + Time.deltaTime * velocity;
        Vector2 nextVelocity = velocity + Time.deltaTime * acceleration;
        float newZ = levelTime * forwardVelocity;

        if (currentMove != Move.NONE) {
            // if we are not handling new input and there is a move currently going on, update it
            bool resetMove = false;
            switch (currentMove) {
                case Move.UP:
                    resetMove = Physics.Raycast(new Vector3(nextPosition.x, nextPosition.y + 0.5f, newZ), Vector3.down, 0.499f, ~LayerMask.GetMask("Player"));
                    break;
                case Move.DOWN:
                    resetMove = Time.realtimeSinceStartup > duckStart + duckDuration;
                    if (resetMove) {
                        animator.SetTrigger("DuckExit");
                        sphereCollider.radius = 0.5f;
                        sphereCollider.center = new Vector3(0.0f, 0.5f, 0f);
                    }
                    break;
                case Move.RIGHT:
                    resetMove = nextPosition.x > lanePosition.x;
                    break;
                case Move.LEFT:
                    resetMove = nextPosition.x < lanePosition.x;
                    break;
            }

            if (resetMove) {
                // a reset consists of zeroing the velocity and currentMove and setting the x position
                // if we reset here there's no need to update physics down below
                velocity = Vector2.zero;
                currentMove = Move.NONE;
                transform.position = new Vector3(lanePosition.x, Mathf.Ceil(nextPosition.y - 0.5f), newZ);
                return;
            }
        }

        // apply new velocity and new position (with calculated z) and update time
        velocity = nextVelocity;
        transform.position = new Vector3(nextPosition.x, nextPosition.y, newZ);

        // if we end up "in the ground", reset y velocity and y position
        RaycastHit hit;
        if (Physics.Raycast(transform.position + 0.5f * Vector3.up, Vector3.down, out hit, 0.499f, ~LayerMask.GetMask("Player"))) {
            velocity = new Vector2(velocity.x, 0.0f);
            transform.position = new Vector3(transform.position.x, Mathf.Ceil(hit.point.y - 0.5f), transform.position.z);
        }

        levelTime += Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (GameManager.gameOver) {
            // ignore events if game is over
            return;
        }

        if (collider.CompareTag("Obstacle") || collider.CompareTag("Destructible")) {
            // collision with obstacle (game over)
            GameOver();
        } else if (collider.CompareTag("Goal")) {
            // collision with goal (win)
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
}