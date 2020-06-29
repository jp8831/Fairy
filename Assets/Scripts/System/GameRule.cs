using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameRule : MonoBehaviour
{
    [SerializeField]
    private string m_uiCanvasName;

    protected GameController m_gameController;
    protected SceneController m_sceneController;
    protected UIController m_uiController;

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
    public abstract void OnPlayEnd ();
}
