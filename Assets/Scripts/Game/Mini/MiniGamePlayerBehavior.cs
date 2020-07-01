using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamePlayerBehavior : PlayerBehavior
{
    [SerializeField]
    private Painter m_painter;

    private MiniGameRule m_rule;

    private void Start ()
    {
        m_rule = GetComponent<MiniGameRule> ();

        PlayerInput.OnMouseButtonDownEventMap.AddListener (0, StartDraw);
        PlayerInput.OnMouseButtonUpEventMap.AddListener (0, EndDraw);
    }

    private void OnDestroy ()
    {
        PlayerInput.OnMouseButtonDownEventMap.RemoveListener (0, StartDraw);
        PlayerInput.OnMouseButtonUpEventMap.RemoveListener (0, EndDraw);
    }

    private void StartDraw ()
    {
        if (m_rule.Finished == false)
        {
            m_painter.DrawEnabled = true;
        }
    }

    private void EndDraw ()
    {
        m_painter.DrawEnabled = false;
    }
}
