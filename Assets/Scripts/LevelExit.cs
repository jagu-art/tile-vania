using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Scene scene = SceneManager.GetActiveScene();
        int currentLevelIndex = scene.buildIndex;
        if(IsTheLastLevel(currentLevelIndex))
        {
            //game over
            Debug.Log("Game Over!");
            yield break;
        }
        Debug.Log("Loading next level!");
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(currentLevelIndex + 1);
    }

    private bool IsTheLastLevel(int index)
    {
        return index >= SceneManager.sceneCountInBuildSettings - 1;
    }
}
