using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public abstract class GameRule : MonoBehaviour
{
    [SerializeField]
    private string m_uiCanvasName;

    private GameController m_gameController;
    private SceneController m_sceneController;
    private UIController m_uiController;
    private AudioManager m_audioManager;

    public GameController GameController
    {
        get { return m_gameController; }
    }

    public SceneController SceneController
    {
        get { return m_sceneController; }
    }

    public UIController UIController
    {
        get { return m_uiController; }
    }

    public AudioManager AudioManager
    {
        get { return m_audioManager; }
    }

    public string UICanvasName
    {
        get { return m_uiCanvasName; }
    }

    private void Awake ()
    {
        var controller = GameObject.FindGameObjectWithTag ("GameController");

        if (controller)
        {
            m_gameController = controller.GetComponent<GameController> ();
            m_gameController.Rule = this;

            m_sceneController = controller.GetComponent<SceneController> ();
            m_uiController = controller.GetComponent<UIController> ();
            m_audioManager = controller.GetComponent<AudioManager> ();
        }
    }

    public virtual void OnPlayStart ()
    {
        m_uiController.ActivateUI (m_uiCanvasName);
    }

    public abstract void OnPlay ();

    public virtual void OnPlayEnd ()
    {
        UIController.DeactivateUI (UICanvasName);
    }
}
