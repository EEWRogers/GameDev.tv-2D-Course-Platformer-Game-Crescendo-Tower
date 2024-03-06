using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    PlayerControls playerControls;
    int currentActiveScene;
    int nextScene;

    void Awake() 
    {
        playerControls = FindObjectOfType<PlayerControls>();
        currentActiveScene = SceneManager.GetActiveScene().buildIndex;
        nextScene = currentActiveScene++;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        playerControls.Freeze();

        yield return new WaitForSeconds(loadDelay);

        SceneManager.LoadScene(nextScene);
    }

}
