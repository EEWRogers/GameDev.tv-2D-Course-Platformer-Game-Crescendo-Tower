using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    PlayerControls playerControls;
    ScenePersist scenePersist;
    int currentActiveScene;
    int nextScene;

    void Awake() 
    {
        playerControls = FindObjectOfType<PlayerControls>();
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Start() 
    {
        scenePersist = FindObjectOfType<ScenePersist>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        playerControls.Freeze();

        yield return new WaitForSeconds(loadDelay);

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            scenePersist.ResetScenePersist();
            SceneManager.LoadScene(nextScene);
        }

        else
        {
            scenePersist.ResetScenePersist();
            SceneManager.LoadScene(0);
        }
        
    }

}
