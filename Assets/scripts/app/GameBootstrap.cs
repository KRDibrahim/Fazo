using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    private void Start()
    {
       
        //load shared ui
        StartCoroutine(LoadSHaredUI());


        // Hardcoded for now — real session-check logic comes later,
        // once the data layer exists. Keep this simple until the
        // navigation skeleton itself is proven to work.
        bool hasSession = false;
        SceneNavigator.Instance.Navigate(hasSession ? GameScene.WhosPlaying : GameScene.Auth, needsBackdrop: true);
    }




    private IEnumerator LoadPersistentScenes()
    {
        var backdropLoad = SceneManager.LoadSceneAsync("Backdrop", LoadSceneMode.Additive);
        while (!backdropLoad.isDone) yield return null;

        var sharedUiLoad = SceneManager.LoadSceneAsync("SharedUI", LoadSceneMode.Additive);
        while (!sharedUiLoad.isDone) yield return null;

        bool hasSession = false;
        GameScene entryPoint = hasSession ? GameScene.WhosPlaying : GameScene.Auth;
        SceneNavigator.Instance.Navigate(entryPoint);
    }

    private IEnumerator LoadSHaredUI()
    {
        var sharedUiLoad = SceneManager.LoadSceneAsync("SharedUI", LoadSceneMode.Additive);
        while (!sharedUiLoad.isDone) yield return null;

    }
}
