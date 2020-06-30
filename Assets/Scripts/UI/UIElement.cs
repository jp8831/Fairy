using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private bool m_bEnableAtStart = true;

    private UIController m_uiController;

    public string ElementID
    {
        get { return gameObject.name; }
    }

    protected UIController UIController
    {
        get { return m_uiController; }
    }

    protected virtual void Awake()
    {
        var gameController = GameObject.FindGameObjectWithTag ("GameController");

        m_uiController = gameController.GetComponent<UIController>();
        m_uiController.RegisterUI(this);
    }

    protected virtual void Start ()
    {
        gameObject.SetActive (m_bEnableAtStart);
    }

    private void OnDestroy()
    {
        m_uiController.UnregisterUI(this);
    }
}
