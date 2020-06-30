using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private string m_uiSceneName;
    [SerializeField]
    private string m_playSceneName;
    [SerializeField]
    private string m_playButtonId;

    private SceneController m_sceneController;
    private UIController m_uiController;
    private GameRule m_rule;
    private bool m_bPlaying;

    public GameRule Rule
    {
        get { return m_rule; }
        set { m_rule = value; }
    }

    private void Start()
    {
        m_sceneController = GetComponent<SceneController>();
        m_uiController = GetComponent<UIController>();

        m_sceneController.LoadScene(m_uiSceneName, true);
        m_sceneController.SceneTransitionEventMap.AddListener(SceneTransitionEvent.OnLoadingComplete, OnSceneLoadComplete);
        m_sceneController.SceneTransitionEventMap.AddListener (SceneTransitionEvent.OnUnloadingComplete, OnSceneUnloadComplete);
    }

    private void Update ()
    {
        if (m_rule && m_bPlaying)
        {
            m_rule.OnPlay ();
        }
    }

    private void StartPlay()
    {
        m_sceneController.LoadScene(m_playSceneName, true);
    }

    public void StopPlay ()
    {
        m_bPlaying = false;
        m_rule.OnPlayEnd ();

        m_sceneController.ActiveSceneName = gameObject.scene.name;
        m_sceneController.UnloadScene(m_playSceneName);
    }

    private void OnSceneLoadComplete (string sceneName, float progress)
    {
        if (sceneName == m_uiSceneName)
        {
            m_uiController.ActivateUI("Canvas_Main Menu");

            var playButton = m_uiController.FindUI<UIButton>(m_playButtonId);
            playButton.AddOnClickListener(StartPlay);
        }

        if (sceneName == m_playSceneName)
        {
            m_uiController.DeactivateUI ("Canvas_Main Menu");
            m_sceneController.ActiveSceneName = m_playSceneName;

            m_rule.OnPlayStart ();
            m_bPlaying = true;
        }
    }

    private void OnSceneUnloadComplete (string sceneName, float progress)
    {
        if (sceneName == m_playSceneName)
        {
            m_uiController.ActivateUI ("Canvas_Main Menu");
        }
    }
}
