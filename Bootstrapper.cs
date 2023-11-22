//using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    async void Start()
    {
        Application.runInBackground = true;
        //await UnityServices.InitializeAsync();

        if (SceneManager.loadedSceneCount == 1)
            SceneManager.LoadScene("CarSelection", LoadSceneMode.Additive);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
#if UNITY_EDITOR
        var currentlyLoadedEditorScene = SceneManager.GetActiveScene();
#endif

        if (SceneManager.GetSceneByName("Bootstrapper").isLoaded != true)
            SceneManager.LoadScene("Bootstrapper");

#if UNITY_EDITOR
        if (currentlyLoadedEditorScene.IsValid())
            SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Additive);
#else
        SceneManager.LoadSceneAsync("CarSelection", LoadSceneMode.Additive);
#endif
    }
}