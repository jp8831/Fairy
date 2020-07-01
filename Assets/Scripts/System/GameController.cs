using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private string m_uiSceneName;
    [SerializeField]
    private string m_playScenePrefix;
    [SerializeField]
    private string m_mainGamePlaySceneName;

    [Header ("UI")]
    [SerializeField]
    private string m_playButtonId;
    [SerializeField]
    private string m_mainMenuUIName;
    [SerializeField]
    private string m_loadingUIName;
    [SerializeField]
    private string m_loadingProgressUIName;
    [SerializeField]
    private string m_loadingTextUIName;
    [SerializeField]
    private string m_loadingCompleteTextUIName;

    private SceneController m_sceneController;

    private UIController m_uiController;
    private UIProgress m_loadingProgress;

    private GameRule m_rule;
    private bool m_bPlaying;

    public GameRule Rule
    {
        get { return m_rule; }
        set { m_rule = value; }
    }

    private void Awake()
    {
        m_sceneController = GetComponent<SceneController>();
        m_uiController = GetComponent<UIController>();
    }

    private void Start ()
    {
        m_sceneController.SceneTransitionEventMap.AddListener (SceneTransitionEvent.OnLoading, OnSceneLoading);
        m_sceneController.SceneTransitionEventMap.AddListener (SceneTransitionEvent.OnLoadingComplete, OnSceneLoadComplete);

        m_sceneController.SceneTransitionEventMap.AddListener (SceneTransitionEvent.OnUnloadingComplete, OnSceneUnloadComplete);

        m_sceneController.LoadScene (m_uiSceneName, true);
    }

    private void Update ()
    {
        if (m_rule && m_bPlaying)
        {
            m_rule.OnPlay ();
        }
    }

    private void OnDestroy ()
    {
        m_sceneController.SceneTransitionEventMap.RemoveListener (SceneTransitionEvent.OnLoading, OnSceneLoading);
        m_sceneController.SceneTransitionEventMap.RemoveListener (SceneTransitionEvent.OnLoadingComplete, OnSceneLoadComplete);
        
        m_sceneController.SceneTransitionEventMap.RemoveListener (SceneTransitionEvent.OnUnloadingComplete, OnSceneUnloadComplete);
    }

    public void StartPlay(string playSceneName)
    {
        if (m_bPlaying)
        {
            StopPlay ();
        }

        m_bPlaying = true;

        m_sceneController.LoadScene(playSceneName, false);
        m_uiController.DeactivateUI (m_mainMenuUIName);

        m_loadingProgress.Value = 0.0f;
        m_uiController.ActivateUI (m_loadingUIName);
        m_uiController.ActivateUI (m_loadingTextUIName);
        m_uiController.DeactivateUI (m_loadingCompleteTextUIName);
    }

    public void StopPlay ()
    {
        m_rule.OnPlayEnd ();
        m_rule = null;

        m_sceneController.ActiveSceneName = gameObject.scene.name;
        m_sceneController.UnloadScene(m_mainGamePlaySceneName);

        m_bPlaying = false;
    }

    private void OnSceneLoading (string sceneName, float progress)
    {
        if (sceneName.StartsWith (m_playScenePrefix))
        {
            if (m_sceneController.ReadyToActivate)
            {
                m_loadingProgress.Value = 1.0f;
                m_uiController.DeactivateUI (m_loadingTextUIName);
                m_uiController.ActivateUI (m_loadingCompleteTextUIName);

                m_sceneController.ActivateScene = Input.anyKey;
            }
            else
            {
                m_loadingProgress.Value = progress;
            }
        }
    }

    private void OnSceneLoadComplete (string sceneName, float progress)
    {
        if (sceneName == m_uiSceneName)
        {
            m_uiController.ActivateUI(m_mainMenuUIName);

            var playButton = m_uiController.FindUI<UIButton>(m_playButtonId);
            playButton.AddOnClickListener(OnClickPlayButton);

            m_loadingProgress = m_uiController.FindUI<UIProgress> (m_loadingProgressUIName);
        }

        if (sceneName.StartsWith (m_playScenePrefix))
        {
            m_uiController.DeactivateUI (m_loadingUIName);
            m_sceneController.ActiveSceneName = m_mainGamePlaySceneName;

            m_rule.OnPlayStart ();
        }
    }

    private void OnSceneUnloadComplete (string sceneName, float progress)
    {
        if (sceneName.StartsWith (m_playScenePrefix) && m_bPlaying == false)
        {
            m_uiController.ActivateUI (m_mainMenuUIName);
        }
    }

    private void OnClickPlayButton ()
    {
        StartPlay (m_mainGamePlaySceneName);
    }
}
