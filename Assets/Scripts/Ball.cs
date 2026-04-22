using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb; // 
    public float bounceForce;

    [Header("Random Bounce Settings")]
    public float minAngle = 30f;   // Min angle from horizontal
    public float maxAngle = 150f;  // Max angle from horizontal

    bool gameStarted = false; // Track if the game has started to prevent bouncing before player input

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 

    }

    void Update()
    {
        if (!gameStarted)  // Wait for player input to start the game
        {
            if (Input.anyKeyDown)
            {
                gameStarted = true;
                StartBounce();
                GameManager.instance.GameStart();
            }
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
        // Center range: 60°-120°, edges can go as sharp as 30° or 150°
        float angle = Mathf.Lerp(150f, 30f, (hitPoint + 1f) / 2f);

        // Add slight randomness on top (+/- 15 degrees)
        angle += Random.Range(-15f, 15f);
        angle = Mathf.Clamp(angle, minAngle, maxAngle); // Never go horizontal

        float radian = angle * Mathf.Deg2Rad;
        Vector2 bounceDir = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

        rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);
    }
}