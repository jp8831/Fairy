using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneTransitionEvent
{
    OnLoadingStart,
    OnLoading,
    OnLoadingComplete,
    OnUnloadingStart,
    OnUnloading,
    OnUnloadingComplete
}

public class SceneController : MonoBehaviour
{
    private EventMap<SceneTransitionEvent, string, float> m_sceneTransitionEventMap;
    private bool m_bOnTransition;

    public EventMap<SceneTransitionEvent, string, float> SceneTransitionEventMap
    {
        get { return m_sceneTransitionEventMap; }
    }

    public string ActiveSceneName
    {
        get { return SceneManager.GetActiveScene ().name; }
        set
        {
            var scene = SceneManager.GetSceneByName (value);

            if (scene.IsValid () && scene.isLoaded)
            {
                SceneManager.SetActiveScene (scene);
            }
        }
    }

    private void Awake()
    {
        m_sceneTransitionEventMap = new EventMap<SceneTransitionEvent, string, float>();
        m_bOnTransition = false;
    }

    public void LoadScene (string sceneName, bool bStartWhenComplete)
    {
        if (m_bOnTransition)
        {
            return;
        }

        m_bOnTransition = true;

        InvokeSceneTransitionEvent(SceneTransitionEvent.OnLoadingStart, sceneName, 0.0f);
        StartCoroutine(LoadSceneCoroutine(sceneName, bStartWhenComplete));
    }

    private IEnumerator LoadSceneCoroutine (string sceneName, bool bStartWhenComplete)
    {
        var loadAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadAsync.allowSceneActivation = bStartWhenComplete;

        while (loadAsync.isDone == false)
        {
            float loadingProgress = loadAsync.progress;
            InvokeSceneTransitionEvent(SceneTransitionEvent.OnLoading, sceneName, loadingProgress);

            yield return null;
        }

        InvokeSceneTransitionEvent (SceneTransitionEvent.OnLoadingComplete, sceneName, 1.0f);
        m_bOnTransition = false;
    }

    public void UnloadScene (string sceneName)
    {
        if (m_bOnTransition)
        {
            return;
        }

        m_bOnTransition = true;

        InvokeSceneTransitionEvent (SceneTransitionEvent.OnUnloadingStart, sceneName, 0.0f);
        StartCoroutine (UnloadSceneCoroutine (sceneName));
    }

    private IEnumerator UnloadSceneCoroutine (string sceneName)
    {
        var unloadAsync = SceneManager.UnloadSceneAsync (sceneName);
        unloadAsync.allowSceneActivation = true;

        while (unloadAsync.isDone == false)
        {
            float loadingProgress = unloadAsync.progress;
            InvokeSceneTransitionEvent (SceneTransitionEvent.OnUnloading, sceneName, loadingProgress);

            yield return null;
        }

        InvokeSceneTransitionEvent (SceneTransitionEvent.OnUnloadingComplete, sceneName, 1.0f);
        m_bOnTransition = false;
    }

    private void InvokeSceneTransitionEvent(SceneTransitionEvent transitionEvent, string sceneName, float progress)
    {
        Action<string, float> onLoading;

        if (m_sceneTransitionEventMap.ListenerMap.TryGetValue(transitionEvent, out onLoading))
        {
            onLoading.Invoke(sceneName, progress);
        }
    }
}
