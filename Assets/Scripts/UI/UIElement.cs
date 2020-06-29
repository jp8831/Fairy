using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private bool m_bEnableAtStart = true;

    private UIController m_gameUI;

    public string ElementID
    {
        get { return gameObject.name; }
    }

    private void Awake()
    {
        gameObject.SetActive(m_bEnableAtStart);

        m_gameUI = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        m_gameUI.RegisterUI(this);
    }

    private void OnDestroy()
    {
        m_gameUI.UnregisterUI(this);
    }
}
