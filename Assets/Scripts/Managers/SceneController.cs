using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{

    public static SceneController instance;
    public Animator loadingScreenAnimator;
    private void Awake()
    {
        Application.targetFrameRate = 60;    
        if(instance != null)
        {
            Debug.LogError("Scene Controller already exists in the scene!");
        }
        instance = this;
    }
   public void MainMenu()
    {
       StartCoroutine(LoadNextScene("Start Scene")); 
    }
    public void StartNewGame()
    {
       StartCoroutine(LoadNextScene("Elysia Room Scene")); 
    }    
    public void ArenaMode()
    {
       StartCoroutine(LoadNextScene("Arena Scene")); 
    }   
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        Debug.Log("Loading previous save unavailable");
    }
    public void Lysandria()
    {
        StartCoroutine(LoadNextScene("Game Scene"));
    }
    public void GameOver()
    {
        //Show Game Over Panel and after its done we reload the scne
       StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().name)); 
    }

    IEnumerator LoadNextScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while(!operation.isDone)
        {
            //GameObjectLoading + animatino
            loadingScreenAnimator.SetTrigger("LoadingOn");
            // progress
            yield return null;
        }
        //play animation for transition
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        if(loadingScreenAnimator != null)
        loadingScreenAnimator.SetTrigger("LoadingOff");
    }
}
