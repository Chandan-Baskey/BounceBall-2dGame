using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputAction startAction;

    [SerializeField] private float bounceForce = 10f;

    [Header("Random Bounce Settings")]
    [SerializeField] private float minAngle = 30f;
    [SerializeField] private float maxAngle = 150f;

    private bool gameStarted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        startAction = new InputAction("Start Game", InputActionType.Button);
        startAction.AddBinding("<Mouse>/leftButton");
        startAction.AddBinding("<Touchscreen>/primaryTouch/press");
        startAction.AddBinding("<Keyboard>/space");
        startAction.AddBinding("<Keyboard>/enter");
        startAction.AddBinding("<Keyboard>/numpadEnter");
        startAction.AddBinding("<Gamepad>/buttonSouth");
        startAction.AddBinding("<Gamepad>/start");
    }

    private void OnEnable()
    {
        startAction.Enable();
    }

    private void OnDisable()
    {
        startAction.Disable();
    }

    private void OnDestroy()
    {
        startAction.Dispose();
    }

    private void Update()
    {
        if (!gameStarted && GameManager.instance != null &&
            GameManager.instance.State == GameState.Waiting && startAction.WasPressedThisFrame())
        {
            gameStarted = true;
            GameManager.instance.GameStart();
            StartBounce();
        }
    }

    private void StartBounce()
    {
        Vector2 randomDirection = GetRandomUpwardDirection();
        rb.velocity = randomDirection * bounceForce;
    }

    // Returns a random direction that always goes UPWARD
    private Vector2 GetRandomUpwardDirection()
    {
        float angle = Random.Range(minAngle, maxAngle);
        float radian = angle * Mathf.Deg2Rad; // Convert to radians for trig functions
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.instance == null || GameManager.instance.State != GameState.Playing)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
            GameManager.instance.EndGame();
        }
        else if (collision.gameObject.CompareTag("Player") && IsValidPlatformHit(collision))
        {
            GameManager.instance.AddScore();
            BounceOffPlatform(collision);
        }
    }

    private bool IsValidPlatformHit(Collision2D collision)
    {
        if (transform.position.y <= collision.collider.bounds.center.y)
        {
            return false;
        }

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private void BounceOffPlatform(Collision2D collision)
    {
        Bounds platformBounds = collision.collider.bounds;
        float normalizedHit = Mathf.InverseLerp(
            platformBounds.min.x,
            platformBounds.max.x,
            transform.position.x);
        float angle = Mathf.Lerp(maxAngle, minAngle, normalizedHit);

        // Add slight randomness on top (+/- 15 degrees)
        angle += Random.Range(-15f, 15f);
        angle = Mathf.Clamp(angle, minAngle, maxAngle); // Never go horizontal

        float radian = angle * Mathf.Deg2Rad;
        Vector2 bounceDirection = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;

        rb.velocity = bounceDirection * bounceForce;
    }

    private void OnValidate()
    {
        bounceForce = Mathf.Max(0.1f, bounceForce);
        minAngle = Mathf.Clamp(minAngle, 5f, 89f);
        maxAngle = Mathf.Clamp(maxAngle, 91f, 175f);
    }
}
