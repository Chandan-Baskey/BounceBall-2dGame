using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    public float bounceForce;

    bool gameStarted= false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted) 
        {
           if(Input.anyKeyDown)
           {
              gameStarted = true;
              StartBounce();
                GameManager.instance.GameStart();
            }
        }
        
    }

    void StartBounce()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), 1).normalized;
        rb.AddForce(randomDirection * bounceForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Respawn"))
            {
                GameManager.instance.Restart();
            }
        else if(collision.gameObject.CompareTag("Player"))
            {
                GameManager.instance.AddScore();
        }
    }

}
