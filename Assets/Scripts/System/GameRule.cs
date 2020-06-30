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
            m_uiController.ActivateUI (m_uiCanvasName);
        }
    }

    public abstract void OnPlayStart ();
    public abstract void OnPlay ();
    public abstract void OnPlayEnd ();
}
