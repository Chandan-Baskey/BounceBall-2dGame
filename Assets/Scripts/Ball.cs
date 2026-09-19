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
        if (!gameStarted && startAction.WasPressedThisFrame())
        {
            gameStarted = true;
            GameManager.instance.GameStart();
            StartBounce();
        }
    }

    void StartBounce()
    {
        rb.velocity = Vector2.zero; // Reset before launching
        Vector2 randomDirection = GetRandomUpwardDirection();
        rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);
    }

    // Returns a random direction that always goes UPWARD
    Vector2 GetRandomUpwardDirection()
    {
        float angle = Random.Range(minAngle, maxAngle);
        float radian = angle * Mathf.Deg2Rad; // Convert to radians for trig functions
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            GameManager.instance.Restart();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AddScore();
            BounceOffPlatform(collision);
        }
    }

    void BounceOffPlatform(Collision2D collision)
    {
        rb.velocity = Vector2.zero; // Stop current velocity

        // Where on the platform did ball hit? (-1 = left edge, 1 = right edge)
        float hitPoint = (transform.position.x - collision.transform.position.x)
                         / collision.collider.bounds.size.x;

        // Map hit position to angle (left hit = goes left, right hit = goes right)
        // Center range: 60�-120�, edges can go as sharp as 30� or 150�
        float angle = Mathf.Lerp(150f, 30f, (hitPoint + 1f) / 2f);

        // Add slight randomness on top (+/- 15 degrees)
        angle += Random.Range(-15f, 15f);
        angle = Mathf.Clamp(angle, minAngle, maxAngle); // Never go horizontal

        float radian = angle * Mathf.Deg2Rad;
        Vector2 bounceDir = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);
    }
}
