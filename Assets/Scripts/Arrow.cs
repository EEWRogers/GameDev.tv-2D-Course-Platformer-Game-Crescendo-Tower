using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 10f;
    Rigidbody2D arrowRigidbody;
    PlayerControls player;
    float xSpeed;
    
    void Awake() 
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerControls>();

        xSpeed = player.transform.localScale.x * arrowSpeed;

        arrowRigidbody.velocity = new Vector2(xSpeed, 0f);
        transform.localScale = player.transform.localScale;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
