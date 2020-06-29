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
        m_sceneController.SceneTransitionEventMap.AddListener(SceneTransitionEvent.OnComplete, OnSceneLoadComplete);
    }

    private void StartGame()
    {
        m_sceneController.LoadScene(m_playSceneName, true);
    }

    private void OnSceneLoadComplete (string sceneName, float progress)
    {
        if (sceneName == m_uiSceneName)
        {
            m_uiController.ActivateUI("Canvas_Main Menu");

            var playButton = m_uiController.FindUI<UIButton>(m_playButtonId);
            playButton.AddOnClickListener(StartGame);
        }

        if (sceneName == m_playSceneName)
        {
            m_uiController.DeactivateUI ("Canvas_Main Menu");
            m_sceneController.ActiveSceneName = m_playSceneName;

            m_rule.OnPlayStart ();
        }
    }
}
