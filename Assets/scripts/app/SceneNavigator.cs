using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameScene
{
    Auth,
    WhosPlaying,
    Landing,
    SectionGate,
    DiscoveryMap,
    SessionActivities,
    GameplayStep,
    KidDetails,
    Settings,
    SharedUI
}

public class SceneNavigator : MonoBehaviour
{


    public static SceneNavigator Instance { get; private set; }

    private GameScene? currentFeatureScene;
     private bool backdropLoaded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

public void Navigate(GameScene target, bool needsBackdrop = true)
{
    StartCoroutine(SwapScene(target, needsBackdrop));
}

private IEnumerator SwapScene(GameScene target, bool needsBackdrop)
{
    if (currentFeatureScene.HasValue)
    {
        var unload = SceneManager.UnloadSceneAsync(currentFeatureScene.Value.ToString());
        while (unload != null && !unload.isDone) yield return null;
    }

    if (needsBackdrop && !backdropLoaded)
    {
        var loadBackdrop = SceneManager.LoadSceneAsync("Backdrop", LoadSceneMode.Additive);
        while (!loadBackdrop.isDone) yield return null;
        backdropLoaded = true;
    }
    else if (!needsBackdrop && backdropLoaded)
    {
        var unloadBackdrop = SceneManager.UnloadSceneAsync("Backdrop");
        while (unloadBackdrop != null && !unloadBackdrop.isDone) yield return null;
        backdropLoaded = false;
    }

    var load = SceneManager.LoadSceneAsync(target.ToString(), LoadSceneMode.Additive);
    while (!load.isDone) yield return null;

    currentFeatureScene = target;
    SceneManager.SetActiveScene(SceneManager.GetSceneByName(target.ToString()));
}
}
