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
    private bool m_bActivateScene;
    private bool m_bReadyToActivate;

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

    public bool ActivateScene
    {
        set { m_bActivateScene = value; }
    }

    public bool ReadyToActivate
    {
        get { return m_bReadyToActivate; }
    }

    private void Awake ()
    {
        m_sceneTransitionEventMap = new EventMap<SceneTransitionEvent, string, float> ();
        m_bOnTransition = false;
    }

    public void LoadScene (string sceneName, bool bAutoStart)
    {
        if (m_bOnTransition)
        {
            return;
        }

        m_bOnTransition = true;

        InvokeSceneTransitionEvent(SceneTransitionEvent.OnLoadingStart, sceneName, 0.0f);
        StartCoroutine(LoadSceneCoroutine(sceneName, bAutoStart));

        if (bAutoStart == false)
        {
            m_bActivateScene = false;
            m_bReadyToActivate = false;
        }
    }

    private IEnumerator LoadSceneCoroutine (string sceneName, bool bAutoStart)
    {
        var loadAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadAsync.allowSceneActivation = bAutoStart;

        while (loadAsync.isDone == false)
        {
            float loadingProgress = loadAsync.progress;

            if (bAutoStart == false)
            {
                if (m_bReadyToActivate)
                {
                    loadAsync.allowSceneActivation = m_bActivateScene;
                }

                m_bReadyToActivate = loadingProgress >= 0.9f;
            }

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
