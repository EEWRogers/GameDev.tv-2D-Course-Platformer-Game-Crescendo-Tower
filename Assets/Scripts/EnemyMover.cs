using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D enemyRigidbody;

    void Awake() 
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            moveSpeed = -moveSpeed;
            FlipEnemySprite();
        }
    }

    void FlipEnemySprite()
    {
        gameObject.transform.localScale = new Vector2 (-Mathf.Sign(enemyRigidbody.velocity.x), 1f);
    }
}
