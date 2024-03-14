using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupAudio;
    [SerializeField] int scoreValue = 100;
    GameSession gameSession;

    void Awake() 
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other is CapsuleCollider2D)
        {
            AudioSource.PlayClipAtPoint(coinPickupAudio, Camera.main.transform.position);
            gameSession.IncreaseScore(scoreValue);
            Destroy(gameObject);
        }
    }

}
