using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    

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
        TouchMove();

    }

    void TouchMove()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(mousePos.x < 0)
            {
                rb.velocity = Vector2.left * speed * Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector2.right * speed * Time.deltaTime;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
